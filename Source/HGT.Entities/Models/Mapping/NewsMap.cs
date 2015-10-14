using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("News");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Alias).HasColumnName("Alias");
            this.Property(t => t.MetaKey).HasColumnName("MetaKey");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.IsHighlight).HasColumnName("IsHighlight");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
        }
    }
}
