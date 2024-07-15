using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductFaultConfiguration : IEntityTypeConfiguration<ProductFault>
    {
        public void Configure(EntityTypeBuilder<ProductFault> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne(e => e.FaultyProduct).WithMany(e => e.ProductFaults).HasForeignKey(e => e.FaultyProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.QualityStandard).WithMany(e => e.ProductFaults).HasForeignKey(e => e.QualityStandardId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionProcessStep).WithMany(e => e.ProductFaults).HasForeignKey(e => e.ProductionProcessStepId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductMeasurement).WithMany(e => e.ProductFaults).HasForeignKey(e => e.ProductMeasurementId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
