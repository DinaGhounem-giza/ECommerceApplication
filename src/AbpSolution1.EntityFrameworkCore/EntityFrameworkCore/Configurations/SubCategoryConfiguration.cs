using AbpSolution1.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations
{
    internal class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        void IEntityTypeConfiguration<SubCategory>.Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories", AbpSolution1Consts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(x => x.NameAr)
                  .IsRequired()
                  .HasMaxLength(AbpSolution1Consts.MaxNameLength);
            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasMaxLength(AbpSolution1Consts.MaxNameLength);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);


        }
    }
}
