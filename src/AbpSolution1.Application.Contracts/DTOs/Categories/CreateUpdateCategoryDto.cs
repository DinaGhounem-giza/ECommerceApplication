using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbpSolution1.DTOs.Categories
{
    public class CreateUpdateCategoryDto
    {
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; }
        [Required]
        [MaxLength(50)]
        public string NameEn { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
