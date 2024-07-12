using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class WarehouseRequestConfiguration : IEntityTypeConfiguration<WarehouseRequest>
    {
        public void Configure(EntityTypeBuilder<WarehouseRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.Status);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne<Staff>().WithMany(e => e.CreatedWarehouseRequests).HasForeignKey(e => e.CreatorId);
            builder.HasOne<Staff>().WithMany(e => e.ReviewedWarehouseRequests).HasForeignKey(e => e.ReviewerId).IsRequired(false);
            builder.HasOne<ProductionRequirement>().WithMany().HasForeignKey(e => e.ProductionRequirementId);
            builder.HasOne<WarehouseTicket>().WithOne(e => e.WarehouseRequest).HasForeignKey<WarehouseTicket>(e => e.WarehouseRequestId).IsRequired(false);
        }
    }
}
