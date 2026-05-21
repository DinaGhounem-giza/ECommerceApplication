using AbpSolution1.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        void IEntityTypeConfiguration<Product>.Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", AbpSolution1Consts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(x => x.NameAr)
                 .IsRequired()
                 .HasMaxLength(AbpSolution1Consts.MaxNameLength);
            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasMaxLength(AbpSolution1Consts.MaxNameLength);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(AbpSolution1Consts.MaxDescriptionLength);
            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(AbpSolution1Consts.MaxDescriptionLength);
            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
            builder.Property(x => x.Stock)
                .IsRequired();


            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
