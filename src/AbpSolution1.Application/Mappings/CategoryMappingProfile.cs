using AbpSolution1.Categories;
using AbpSolution1.DTOs.Categories;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace AbpSolution1.Mappings
{

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CategoryToCategoryDtoMapper : MapperBase<Category, CategoryDto>
    {
        public override partial CategoryDto Map(Category source);

        public override partial void Map(Category source, CategoryDto destination);

    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreateUpdateCategoryDtoToCategoryMapper : MapperBase<CreateUpdateCategoryDto, Category>
    {
        public override partial Category Map(CreateUpdateCategoryDto source);

        public override partial void Map(CreateUpdateCategoryDto source, Category destination);
    }
}
