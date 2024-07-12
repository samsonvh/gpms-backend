using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class WarehouseTickerConfiguration : IEntityTypeConfiguration<WarehouseTicket>
    {
        public void Configure(EntityTypeBuilder<WarehouseTicket> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.Type);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne<WarehouseRequest>().WithOne(e => e.WarehouseTicket).HasForeignKey<WarehouseTicket>(e => e.WarehouseRequestId);
            builder.HasOne<Warehouse>().WithMany().HasForeignKey(e => e.WarehouseId);
            builder.HasOne<ProductSpecification>().WithMany().HasForeignKey(e => e.ProductSpecificationId);
        }
    }
}
