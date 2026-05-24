using AbpSolution1.DTOs.Categories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Categories
{
    public interface ICategoryAppService : ICrudAppService<
        CategoryDto, int, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>
    {
       
    }
}
