using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class StoreMap : EntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(250);

            this.Property(t => t.Address)
                .HasMaxLength(250);

            this.Property(t => t.Phone)
                .HasMaxLength(25);

            this.Property(t => t.OpenTime)
                .HasMaxLength(50);

            this.Property(t => t.Picture)
                .HasMaxLength(50);

            this.Property(t => t.Alias)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Stores");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.OpenTime).HasColumnName("OpenTime");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Alias).HasColumnName("Alias");
        }
    }
}
