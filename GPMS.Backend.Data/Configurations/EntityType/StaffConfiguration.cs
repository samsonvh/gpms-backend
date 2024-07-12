using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.FullName).HasMaxLength(100);
            builder.Property(e => e.Position);
            builder.Property(e => e.Status);

            builder.HasOne<Account>().WithOne(e => e.Staff).HasForeignKey<Staff>(e => e.AccountId);
            builder.HasOne<Department>().WithMany(e => e.Staffs).HasForeignKey(e => e.DepartmentId);

            builder.HasMany<Product>().WithOne(e => e.Creator).HasForeignKey(e => e.CreatorId);
            builder.HasMany<Product>().WithOne(e => e.Reviewer).HasForeignKey(e => e.ReviewerId).IsRequired(false);
            builder.HasMany<ProductionPlan>().WithOne(e => e.Creator).HasForeignKey(e => e.CreatorId);
            builder.HasMany<ProductionPlan>().WithOne(e => e.Reviewer).HasForeignKey(e => e.ReviewerId).IsRequired(false);
            builder.HasMany<InspectionRequest>().WithOne(e => e.Creator).HasForeignKey(e => e.CreatorId).IsRequired(false);
            builder.HasMany<InspectionRequest>().WithOne(e => e.Reviewer).HasForeignKey(e => e.ReviewerId).IsRequired(false);
            builder.HasMany<WarehouseRequest>().WithOne(e => e.Creator).HasForeignKey(e => e.CreatorId).IsRequired(false);
            builder.HasMany<WarehouseRequest>().WithOne(e => e.Reviewer).HasForeignKey(e => e.ReviewerId).IsRequired(false);
        }
    }
}
