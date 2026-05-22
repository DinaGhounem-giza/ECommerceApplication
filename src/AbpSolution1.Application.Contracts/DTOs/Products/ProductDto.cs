using Volo.Abp.Application.Dtos;

namespace AbpSolution1.DTOs.Products
{
    public class ProductDto : EntityDto<int>
    {
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get;  set; }
        public int Stock { get;  set; }
        public ProductDto() { }
    }
}
