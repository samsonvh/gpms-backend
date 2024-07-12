using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class QualityStandardConfiguration : IEntityTypeConfiguration<QualityStandard>
    {
        public void Configure(EntityTypeBuilder<QualityStandard> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne<ProductSpecification>().WithMany().HasForeignKey(e => e.ProductSpecificationId);
            builder.HasOne<Material>().WithMany().HasForeignKey(e => e.MaterialId).IsRequired(false);

            builder.HasMany<ProductFault>().WithOne().HasForeignKey(e => e.QualityStandardId);
        }
    }
}
