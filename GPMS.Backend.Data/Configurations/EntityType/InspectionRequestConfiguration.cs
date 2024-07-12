using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class InspectionRequestConfiguration : IEntityTypeConfiguration<InspectionRequest>
    {
        public void Configure(EntityTypeBuilder<InspectionRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.Status);

            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.CreatorId);
            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.ReviewerId).IsRequired(false);
            builder.HasOne<ProductionSeries>().WithMany().HasForeignKey(e => e.ProductionSeriesId);
            builder.HasOne<InspectionRequestResult>().WithOne(e => e.InspectionRequest).HasForeignKey<InspectionRequestResult>(e => e.InspectionRequestId).IsRequired(false);
        }
    }
}
