using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class EmailTemplateMap : EntityTypeConfiguration<EmailTemplate>
    {
        public EmailTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.EmailTemplateId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("EmailTemplates");
            this.Property(t => t.EmailTemplateId).HasColumnName("EmailTemplateId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Deleteable).HasColumnName("Deleteable");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
        }
    }
}
