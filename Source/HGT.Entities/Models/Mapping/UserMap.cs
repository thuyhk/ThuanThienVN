using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PasswordSalt)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Company)
                .HasMaxLength(255);

            this.Property(t => t.Phone)
                .HasMaxLength(255);

            this.Property(t => t.Mobile)
                .HasMaxLength(255);

            this.Property(t => t.Image)
                .HasMaxLength(255);

            this.Property(t => t.TokenForgotPassWord)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Company).HasColumnName("Company");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.IsAdmin).HasColumnName("IsAdmin");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.LastLogonDate).HasColumnName("LastLogonDate");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.TokenForgotPassWord).HasColumnName("TokenForgotPassWord");
        }
    }
}
