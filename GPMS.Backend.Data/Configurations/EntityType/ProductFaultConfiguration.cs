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
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne<FaultyProduct>().WithMany().HasForeignKey(e => e.FaultyProductId);
            builder.HasOne<QualityStandard>().WithMany().HasForeignKey(e => e.QualityStandardId);
            builder.HasOne<ProductionProcessStep>().WithMany().HasForeignKey(e => e.ProductionProcessStepId);
            builder.HasOne<Measurement>().WithOne().HasForeignKey<ProductFault>(e => e.ProductMeasurementId).IsRequired(false);
        }
    }
}
