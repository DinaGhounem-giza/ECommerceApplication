using System;
using AbpSolution1.Entities.Products;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Orders
{
    public class OrderDetails : AuditedEntity<Guid>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; private set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public void setQuantity(int quantity)
        {
            if(quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            Quantity = quantity;
        }
    }
}
