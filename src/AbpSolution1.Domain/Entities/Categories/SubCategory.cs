using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSolution1.Entities.Categories
{
    public class SubCategory : FullAuditedEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public SubCategory(int id, string nameAr, string nameEn, int categoryId) : base(id)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            CategoryId = categoryId;
        }
        public SubCategory() { }

    }
}
