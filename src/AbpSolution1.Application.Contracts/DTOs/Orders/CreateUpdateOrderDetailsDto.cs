using System.ComponentModel.DataAnnotations;

namespace AbpSolution1.DTOs.Orders
{
    public class CreateUpdateOrderDetailsDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Quantity Must be greater than 0")]
        public int Quantity { get; set; }
    }
}
