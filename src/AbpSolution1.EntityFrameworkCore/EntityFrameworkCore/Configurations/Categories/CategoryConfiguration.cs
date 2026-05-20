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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        void IEntityTypeConfiguration<Category>.Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", GeneralConfigurations.SchemaName);

            builder.ConfigureByConvention();

            builder.Property(x => x.NameAr)
                   .IsRequired()
                   .HasMaxLength(GeneralConfigurations.MaxNameLength);
            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasMaxLength(GeneralConfigurations.MaxNameLength);
        }
    }
}
