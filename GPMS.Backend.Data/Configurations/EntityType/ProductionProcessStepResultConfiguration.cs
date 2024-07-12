using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionProcessStepResultConfiguration : IEntityTypeConfiguration<ProductionProcessStepResult>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessStepResult> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.Status);

            builder.HasOne<InspectionRequestResult>().WithOne().HasForeignKey<ProductionProcessStepResult>(e => e.InspectionRequestResultId).IsRequired(false);
            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.CreatorId);
            builder.HasOne<ProductionSeries>().WithMany().HasForeignKey(e => e.ProductionSeriesId);

            builder.HasMany<ProductionProcessStepIOResult>().WithOne().HasForeignKey(e => e.StepResultId);
        }
    }
}
