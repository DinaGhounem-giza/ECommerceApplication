using System;
using System.Collections.Generic;
using System.Text;
using AbpSolution1.Configurations;
using AbpSolution1.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations.Categories
{
    internal class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        void IEntityTypeConfiguration<SubCategory>.Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories",GeneralConfigurations.SchemaName);
            builder.ConfigureByConvention();

            builder.Property(x => x.NameAr)
                  .IsRequired()
                  .HasMaxLength(GeneralConfigurations.MaxNameLength);
            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasMaxLength(GeneralConfigurations.MaxNameLength);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);


        }
    }
}
