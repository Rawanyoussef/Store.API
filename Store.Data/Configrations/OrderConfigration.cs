
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntity;

namespace Store.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            
            builder.OwnsOne(order => order.ShippingAddress, sa =>
            {
                sa.WithOwner();
            });

            builder.HasMany(order => order.Items) 
                   .WithOne()
                   .HasForeignKey(item => item.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);  // Fixed typo here
        }
    }
}
