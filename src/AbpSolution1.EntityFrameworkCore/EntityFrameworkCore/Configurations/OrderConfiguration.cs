using AbpSolution1.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        void IEntityTypeConfiguration<Order>.Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", AbpSolution1Consts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(o => o.Id).ValueGeneratedNever();

            builder.HasMany(o => o.Details)
                .WithOne()
                .HasForeignKey(od => od.OrderId)
                .IsRequired();
        }
    }
}
