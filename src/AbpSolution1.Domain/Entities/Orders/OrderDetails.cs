using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace AbpSolution1.Entities.Orders
{
    public class OrderDetails : Entity<Guid>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}
