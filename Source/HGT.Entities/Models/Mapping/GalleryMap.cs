using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class GalleryMap : EntityTypeConfiguration<Gallery>
    {
        public GalleryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.ObjectId);
            this.Property(t => t.Name);
            this.Property(t => t.Alias);
            this.Property(t => t.Type);
            this.Property(t => t.Category);
            this.Property(t => t.CreatedDate);

            // Table
            this.ToTable("Gallery");
        }
    }
}
