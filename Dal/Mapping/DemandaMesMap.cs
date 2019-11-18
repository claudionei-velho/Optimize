using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DemandaMesMap : EntityTypeConfiguration<DemandaMes> {
    public DemandaMesMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.Ano, t.Mes, t.Categoria });

      // Table, Properties & Column Mappings
      this.ToTable("DemandaMensal", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Ano).HasColumnName("Ano").IsRequired();
      this.Property(t => t.Mes).HasColumnName("Mes").IsRequired();
      this.Property(t => t.Categoria).HasColumnName("Categoria").IsRequired();
      this.Property(t => t.Rateio).HasColumnName("Rateio").HasPrecision(9, 3);
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Equivalente).HasColumnName("Equivalente");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.DemandasMes).HasForeignKey(k => k.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.TCategoria)
          .WithMany(f => f.DemandasMes).HasForeignKey(f => f.Categoria)
          .WillCascadeOnDelete(false);
    }
  }
}
