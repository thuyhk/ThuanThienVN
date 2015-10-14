using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class GalleryDetailMap : EntityTypeConfiguration<GalleryDetail>
    {
        public GalleryDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
           
            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.FileName);
            this.Property(t => t.Url);
            this.Property(t => t.Thumbnail);
            this.Property(t => t.IsVideo);
            this.Property(t => t.GalleryId);
            this.Property(t => t.DisplayOrder);

            // Table
            this.ToTable("GalleryDetail");

            // Relationships
            this.HasRequired(t => t.Gallery)
                .WithMany(t => t.GalleryDetails)
                .HasForeignKey(d => d.GalleryId)
                .WillCascadeOnDelete(false);
            
        }
    }
}
