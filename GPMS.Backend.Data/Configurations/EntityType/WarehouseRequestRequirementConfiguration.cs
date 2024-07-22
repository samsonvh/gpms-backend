using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class WarehouseRequestRequirementConfiguration : IEntityTypeConfiguration<WarehouseRequestRequirement>
    {
        public void Configure(EntityTypeBuilder<WarehouseRequestRequirement> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Quantity);

            builder.HasOne(e => e.ProductionRequirement)
                .WithMany(e => e.WarehouseRequestRequirements)
                .HasForeignKey(e => e.ProductionRequirementId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.WarehouseRequest)
                .WithMany(e => e.WarehouseRequestRequirements)
                .HasForeignKey(e => e.WarehouseRequestId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
