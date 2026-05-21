using System;
using System.Collections.Generic;
using System.Text;
using AbpSolution1.DTOs.Categories;

namespace AbpSolution1.DTOs.Products
{
    public class ProductDto
    {
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public CategoryDto Category { get; set; }
    }
}
