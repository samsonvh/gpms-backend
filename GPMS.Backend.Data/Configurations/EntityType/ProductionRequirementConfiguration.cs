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
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Quantity);

            builder.HasOne(e => e.ProductSpecification)
                .WithMany(e => e.ProductionRequirements)
                .HasForeignKey(e => e.ProductSpecificationId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionPlan)
                .WithMany(e => e.ProductionRequirements)
                .HasForeignKey(e => e.ProductionPlanId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
