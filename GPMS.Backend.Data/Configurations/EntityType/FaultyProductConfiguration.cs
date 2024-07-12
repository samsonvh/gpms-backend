using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class FaultyProductConfiguration : IEntityTypeConfiguration<FaultyProduct>
    {
        public void Configure(EntityTypeBuilder<FaultyProduct> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ProductOrderNumber);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne<InspectionRequestResult>().WithMany().HasForeignKey(e => e.InspectionRequestResultId);
            builder.HasOne<ProductSpecification>().WithMany().HasForeignKey(e => e.SpecificationId);

            builder.HasMany<ProductFault>().WithOne().HasForeignKey(e => e.FaultyProductId);
        }
    }
}
