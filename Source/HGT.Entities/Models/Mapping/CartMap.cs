using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class CartMap : EntityTypeConfiguration<Cart>
    {
        public CartMap()
        {
            // Primary Key
            this.HasKey(t => t.RecordId);

            // Properties
            this.Property(t => t.CartId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Carts");
            this.Property(t => t.RecordId).HasColumnName("RecordId");
            this.Property(t => t.CartId).HasColumnName("CartId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Count).HasColumnName("Count");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
