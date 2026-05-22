using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.DTOs.Orders
{
    public class CreateUpdateOrderDto
    {
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; }
        [Required]
        public List<CreateUpdateOrderDetailsDto> Details { get; set; }

    }
}
