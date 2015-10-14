using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Categories");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Alias).HasColumnName("Alias");
            this.Property(t => t.MetaKey).HasColumnName("MetaKey");
            this.Property(t => t.RootId).HasColumnName("RootId");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.Thumbnail).HasColumnName("Thumbnail");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsFeature).HasColumnName("IsFeature");
            this.Property(t => t.Brief).HasColumnName("Brief");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.KeyBenefit).HasColumnName("KeyBenefit");
            this.Property(t => t.Solution).HasColumnName("Solution");
            this.Property(t => t.HowIt).HasColumnName("HowIt");
            this.Property(t => t.EnablePrintCapbility).HasColumnName("EnablePrintCapbility");
            this.Property(t => t.EnableGallery).HasColumnName("EnableGallery");
        }
    }
}
