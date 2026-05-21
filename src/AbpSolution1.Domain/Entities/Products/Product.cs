using System;
using AbpSolution1.Entities.Categories;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Products
{
    public class Product : FullAuditedEntity<int>
    {
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public Product() { }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero");
            }
            Price = price;
        }

        public void SetStock(int stock)
        {
            if (stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative");
            }
            Stock = stock;

        }
    }
}
