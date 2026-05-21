using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Products;
using AbpSolution1.Products;
using Volo.Abp.Application.Services;

namespace AbpSolution1
{
    public class ProductService : ApplicationService, IProductService
    {
        Task IProductService.CreateProduct(ProductDto product)
        {
            throw new NotImplementedException();
        }

        Task IProductService.DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        Task<ProductDto> IProductService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<ProductDto>> IProductService.GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        Task IProductService.UpdateProduct(ProductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
