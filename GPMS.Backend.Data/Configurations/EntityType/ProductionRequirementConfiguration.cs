using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionRequirementConfiguration : IEntityTypeConfiguration<ProductionRequirement>
    {
        public void Configure(EntityTypeBuilder<ProductionRequirement> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.OvertimeQuantity);
            builder.Property(e => e.Quarter).IsRequired(false);
            builder.Property(e => e.Month).IsRequired(false);
            builder.Property(e => e.Batch).IsRequired(false);
            builder.Property(e => e.Day).IsRequired(false);

            builder.HasOne<ProductSpecification>().WithMany().HasForeignKey(e => e.ProductSpecificationId);
            builder.HasOne<ProductionPlan>().WithMany().HasForeignKey(e => e.ProductionPlanId);

            builder.HasMany<ProductionEstimation>().WithOne().HasForeignKey(e => e.ProductionRequirementId);
            builder.HasMany<WarehouseRequest>().WithOne().HasForeignKey(e => e.ProductionRequirementId);
        }
    }
}
