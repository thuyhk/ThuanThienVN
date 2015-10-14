using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class EmailQueueMap : EntityTypeConfiguration<EmailQueue>
    {
        public EmailQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("EmailQueues");
            this.Property(t => t.ID);
            this.Property(t => t.Active);
            this.Property(t => t.EmailSubject);
            this.Property(t => t.DisplayNameFrom);
            this.Property(t => t.EmailFrom);
            this.Property(t => t.DisplayNameTo);
            this.Property(t => t.EmailTo);
            this.Property(t => t.EmailCc);
            this.Property(t => t.EmailBcc);
            this.Property(t => t.EmailBody);
            this.Property(t => t.Department);
            this.Property(t => t.EmailContact);
            this.Property(t => t.PhoneContact);
            this.Property(t => t.CompanyName);
            this.Property(t => t.Note);
            this.Property(t => t.SenderIP);
            this.Property(t => t.CreatedDate);
            this.Property(t => t.UpdatedDate);
            this.Property(t => t.SendBy);
        }
    }
}
