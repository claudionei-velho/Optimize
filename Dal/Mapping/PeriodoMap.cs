using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PeriodoMap : EntityTypeConfiguration<Periodo> {
    public PeriodoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(32);

      // Table & Column Mappings
      this.ToTable("Periodos");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
    }
  }
}
