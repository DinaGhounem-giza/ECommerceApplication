using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Categories
{
    public class Category : FullAuditedAggregateRoot<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public Category()
        {
        }

        public Category(int id, string nameAr, string nameEn) :base(id)
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }
    }
}
