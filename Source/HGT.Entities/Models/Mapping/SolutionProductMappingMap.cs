using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HGT.Entities.Models.Mapping
{
    public class SolutionProductMappingMap : EntityTypeConfiguration<SolutionProductMapping>
    {
        public SolutionProductMappingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);          

            // Table & Column Mappings
            this.ToTable("SolutionProductMapping");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.SolutionId).HasColumnName("SolutionId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
