using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class SolutionMap : EntityTypeConfiguration<Solution>
    {
        public SolutionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Solutions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Alias).HasColumnName("Alias");
            this.Property(t => t.MetaKey).HasColumnName("MetaKey");
            this.Property(t => t.RootId).HasColumnName("RootId");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.Thumbnail).HasColumnName("Thumbnail");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsFeature).HasColumnName("IsFeature");
            this.Property(t => t.Brief).HasColumnName("Brief");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.EnableGallery).HasColumnName("EnableGallery");
            this.Property(t => t.OpenLink).HasColumnName("OpenLink");
            this.Property(t => t.Link).HasColumnName("Link");
        }
    }
}
