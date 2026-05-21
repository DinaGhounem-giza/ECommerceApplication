using AbpSolution1.DTOs.Categories;
using AbpSolution1.Entities.Categories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Categories
{
    public class CategoryAppService : CrudAppService<
        Category,
        CategoryDto,
        int,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto>,
        ICategoryAppService
    {
        public CategoryAppService(IRepository<Category, int> repository) : base(repository)
        {
        }
    }
}
