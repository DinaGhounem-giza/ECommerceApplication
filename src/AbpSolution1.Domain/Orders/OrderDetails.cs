using System;
using AbpSolution1.Products;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Orders
{
    public class OrderDetails : AuditedEntity<Guid>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; private set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public OrderDetails(int productId,int quantity)
        {
            ProductId = productId;
            SetQuantity(quantity);

        }


        public void SetQuantity(int quantity)
        {
            if(quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            Quantity = quantity;
        }
    }
}
