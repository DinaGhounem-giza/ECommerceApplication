using System;
using AbpSolution1.Entities.Categories;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Products
{
    public class Product : FullAuditedAggregateRoot<int>
    {
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public Product() { }

        public void setPrice(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero");
            }
            else
            {
                Price = price;
            }
        }

        public void setStock(int stock)
        {
            if (stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative");
            }
            else
            {
                Stock = stock;
            }

        }
    }
}
