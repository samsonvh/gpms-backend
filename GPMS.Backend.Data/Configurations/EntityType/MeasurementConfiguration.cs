using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
    {
        public void Configure(EntityTypeBuilder<Measurement> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Unit).HasMaxLength(100);
            builder.Property(e => e.Measure);

            builder.HasOne(e => e.ProductSpecification).WithMany(e => e.Measurements).HasForeignKey(e => e.ProductSpecificationId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
