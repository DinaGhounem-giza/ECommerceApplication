using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Products;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Products
{
    public class ProductAppService : CrudAppService<Product,ProductDto, int, 
        PagedAndSortedResultRequestDto, CreateUpdateProductDto>,
        IProductAppService
    {
        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
        }
    }
}
