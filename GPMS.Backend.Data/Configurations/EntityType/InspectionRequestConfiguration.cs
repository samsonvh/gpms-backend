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
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.Status);

            builder.HasOne(e => e.Creator).WithMany(e => e.CreatedInspectionRequests).HasForeignKey(e => e.CreatorId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Reviewer).WithMany(e => e.ReviewedInspectionRequests).HasForeignKey(e => e.ReviewerId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionSeries).WithMany(e => e.InspectionRequests).HasForeignKey(e => e.ProductionSeriesId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
