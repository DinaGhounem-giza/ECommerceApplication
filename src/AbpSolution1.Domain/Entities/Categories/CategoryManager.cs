using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AbpSolution1.Entities.Categories
{
    public class CategoryManager : DomainService
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryManager(IRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task SoftDeleteCategoryWithChildrenAsync(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(categoryId);

            await SoftDeleteCategoryRecursiveAsync(category);
        }

        private async Task SoftDeleteCategoryRecursiveAsync(Category category)
        {
            // Get all child categories
            var childCategories = await _categoryRepository.GetListAsync(x => x.ParentCategoryId == category.Id);

            // Recursively soft delete all children
            foreach (var child in childCategories)
            {
                await SoftDeleteCategoryRecursiveAsync(child);
            }

            // Soft delete the current category
            await _categoryRepository.DeleteAsync(category, autoSave: false);
        }

        public async Task SoftDeleteCategoryWithChildrenAsync(Category category)
        {
            await SoftDeleteCategoryRecursiveAsync(category);
        }
    }
}
