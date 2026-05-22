using System;
using AbpSolution1.Categories;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Products
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



        public Product(string nameAr, string descriptionAr, string nameEn, string descriptionEn, decimal price, int stock, int categoryId)
        {
            NameAr = nameAr;
            DescriptionAr = descriptionAr;
            NameEn = nameEn;
            DescriptionEn = descriptionEn;
            SetPrice(price);
            SetStock(stock);
            CategoryId = categoryId;
        }

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
