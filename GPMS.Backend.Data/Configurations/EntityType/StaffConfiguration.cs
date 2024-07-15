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
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.FullName).HasMaxLength(100);
            builder.Property(e => e.Position);
            builder.Property(e => e.Status);

            builder.HasOne(e => e.Account).WithOne(e => e.Staff).HasForeignKey<Staff>(e => e.AccountId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Department).WithMany(e => e.Staffs).HasForeignKey(e => e.DepartmentId).IsRequired(false);
            
        }
    }
}
