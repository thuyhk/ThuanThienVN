using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);            

            // Table & Column Mappings
            this.ToTable("Products");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SKU).HasColumnName("SKU");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Alias).HasColumnName("Alias");
            this.Property(t => t.Brief).HasColumnName("Brief");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.Thumbnail).HasColumnName("Thumbnail");
            this.Property(t => t.CallPrice).HasColumnName("CallPrice");
            this.Property(t => t.KeyBenefit).HasColumnName("KeyBenefit");
            this.Property(t => t.Capability).HasColumnName("Capability");
            this.Property(t => t.CaseStudy).HasColumnName("CaseStudy");
            this.Property(t => t.EnableGallery).HasColumnName("EnableGallery");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.FileAttach).HasColumnName("FileAttach");
            this.Property(t => t.FileAttachName).HasColumnName("FileAttachName");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}
