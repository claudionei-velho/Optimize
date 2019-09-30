using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PtOrigemMap : EntityTypeConfiguration<PtOrigem> {
    public PtOrigemMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Sentido)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Prefixo)
          .IsRequired().HasMaxLength(16);

      this.Property(t => t.Identificacao)
          .IsRequired().HasMaxLength(32);

      // Table & Column Mappings
      this.ToTable("PtOrigem", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo");
      this.Property(t => t.Identificacao).HasColumnName("Identificacao");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.PtOrigens).HasForeignKey(d => d.LinhaId);

      this.HasRequired(t => t.Ponto)
          .WithMany(t => t.PtOrigens).HasForeignKey(d => d.PontoId);
    }
  }
}
