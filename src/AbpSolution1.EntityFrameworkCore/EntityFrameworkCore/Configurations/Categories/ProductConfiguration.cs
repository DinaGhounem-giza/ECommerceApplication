using System;
using System.Collections.Generic;
using System.Text;
using AbpSolution1.Configurations;
using AbpSolution1.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations.Categories
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        void IEntityTypeConfiguration<Product>.Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", GeneralConfigurations.SchemaName);
            builder.ConfigureByConvention();

            builder.Property(x => x.NameAr)
                 .IsRequired()
                 .HasMaxLength(GeneralConfigurations.MaxNameLength);
            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasMaxLength(GeneralConfigurations.MaxNameLength);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(GeneralConfigurations.MaxDescriptionLength);
            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(GeneralConfigurations.MaxDescriptionLength);
            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
            builder.Property(x => x.Stock)
                .IsRequired();


            builder.HasOne(x => x.SubCategory)
                .WithMany()
                .HasForeignKey(x => x.SubCategoryId);
        }
    }
}
