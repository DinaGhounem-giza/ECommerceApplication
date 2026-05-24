using AbpSolution1.DTOs.Products;
using AbpSolution1.Products;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace AbpSolution1.Mappings
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProductToProductDtoMapper : MapperBase<Product,ProductDto>
    {
        public override partial ProductDto Map(Product source);

        public override partial void Map(Product source, ProductDto destination);
    }


    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreateUpdateProductDtoToProductMapper : MapperBase<CreateUpdateProductDto, Product>
    {
        public override partial Product Map(CreateUpdateProductDto source);

        public override partial void Map(CreateUpdateProductDto source, Product destination);
    }
}
