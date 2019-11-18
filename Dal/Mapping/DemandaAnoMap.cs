using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DemandaAnoMap : EntityTypeConfiguration<DemandaAno> {
    public DemandaAnoMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.Ano, t.Categoria });

      // Table, Properties & Column Mappings
      this.ToTable("DemandaAnual", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Ano).HasColumnName("Ano").IsRequired();
      this.Property(t => t.Categoria).HasColumnName("Categoria").IsRequired();
      this.Property(t => t.Rateio).HasColumnName("Rateio").HasPrecision(9, 3);
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Equivalente).HasColumnName("Equivalente");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.DemandasAno).HasForeignKey(k => k.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.TCategoria)
          .WithMany(f => f.DemandasAno).HasForeignKey(f => f.Categoria)
          .WillCascadeOnDelete(false);
    }
  }
}
