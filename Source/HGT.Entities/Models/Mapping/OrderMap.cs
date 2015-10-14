using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CustomerName)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(25);

            this.Property(t => t.Email)
                .HasMaxLength(150);

            this.Property(t => t.Address)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Orders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.OrderStatusId).HasColumnName("OrderStatusId");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.StateProvinceId).HasColumnName("StateProvinceId");
            this.Property(t => t.DistrictId).HasColumnName("DistrictId");
            this.Property(t => t.Deleted).HasColumnName("Deleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.HomeDelivery).HasColumnName("HomeDelivery");
        }
    }
}
