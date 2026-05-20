using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Orders
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; }
        public List<OrderDetails> Details { get; set; } = new List<OrderDetails>();
        

    }
}
