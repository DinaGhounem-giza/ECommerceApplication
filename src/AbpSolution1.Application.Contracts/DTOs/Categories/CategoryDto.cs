using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.DTOs.Categories
{
    public  class CategoryDto : EntityDto<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public CategoryDto? ParentCategory { get; set; }

        public CategoryDto(){ }
        
    }
}
