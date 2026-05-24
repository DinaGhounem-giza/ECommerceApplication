using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.DTOs.Orders
{
    public class OrderDto : EntityDto<Guid>
    {
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; }

        public List<OrderDetailsDto> Details { get; set; }

        public OrderDto() { }
    }
}
