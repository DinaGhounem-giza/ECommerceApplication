using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbpSolution1.DTOs.Products;
using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Products
{
    [Authorize(Roles = "SystemAdmin")]
    public class ProductAppService : CrudAppService<Product,ProductDto, int, 
        PagedAndSortedResultRequestDto, CreateUpdateProductDto>,
        IProductAppService
    {
        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
        }
    }
}
