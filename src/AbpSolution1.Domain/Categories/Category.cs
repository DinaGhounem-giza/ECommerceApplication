using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Categories
{
    public class Category : FullAuditedEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

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
