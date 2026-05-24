using AbpSolution1.DTOs.Categories;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Categories
{
    [Authorize(Roles = "SystemAdmin")]
    public class CategoryAppService : CrudAppService<Category,CategoryDto,int,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto>,
        ICategoryAppService
    {
        public CategoryAppService(IRepository<Category, int> repository) : base(repository)
        {
        }
    }
}
