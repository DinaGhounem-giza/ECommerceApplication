using AbpSolution1.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpSolution1.EntityFrameworkCore.Configurations
{
    internal class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        void IEntityTypeConfiguration<OrderDetails>.Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("OrderDetails", AbpSolution1Consts.DbSchema);
            builder.ConfigureByConvention();

            builder.HasOne(od => od.Order)
                   .WithMany()
                   .HasForeignKey(od => od.OrderId);

            builder.HasOne(od => od.Product)
                   .WithMany()
                   .HasForeignKey(od => od.ProductId);
        }
    }
}
