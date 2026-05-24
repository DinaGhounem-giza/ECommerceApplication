using System;
using System.ComponentModel.DataAnnotations;

namespace AbpSolution1.DTOs.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; }

        [MaxLength(200)]
        public string DescriptionAr { get; set; }

        [Required]
        [MaxLength(50)]
        public string NameEn { get; set; }

        [MaxLength(200)]
        public string DescriptionEn { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0.")]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public CreateUpdateProductDto() { }
    }
}
