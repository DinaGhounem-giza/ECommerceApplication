using System.Collections.Generic;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Products;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Products
{
    public interface IProductService : IApplicationService
    {
        public Task<List<ProductDto>> GetProductsAsync();
        public Task<ProductDto> GetProductByIdAsync(int id);
        public Task CreateProduct(ProductDto product);
        public Task UpdateProduct(ProductDto product);
        public Task DeleteProduct(int id);
    }
}
