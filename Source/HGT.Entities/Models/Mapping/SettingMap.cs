using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Settings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Values).HasColumnName("Values");
            this.Property(t => t.ContactPage).HasColumnName("ContactPage");
            this.Property(t => t.ContactOss).HasColumnName("ContactOss");
            this.Property(t => t.IntroPage).HasColumnName("IntroPage");
        }
    }
}
