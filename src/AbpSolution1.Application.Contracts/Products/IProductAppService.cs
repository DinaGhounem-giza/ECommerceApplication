using AbpSolution1.DTOs.Products;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Products
{
    public interface IProductAppService : ICrudAppService<ProductDto, int,
        PagedAndSortedResultRequestDto,CreateUpdateProductDto>
    {
        
    }
}
