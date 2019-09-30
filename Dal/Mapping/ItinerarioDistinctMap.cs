using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ItinerarioDistinctMap : EntityTypeConfiguration<ItinerarioDistinct> {
    public ItinerarioDistinctMap() {
      // Primary Key
      this.HasKey(t => new { t.EmpresaId, t.LinhaId, t.Prefixo, t.Sentido });

      // Table, Properties & Column Mappings
      this.ToTable("ItinerariosDistinct", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").IsRequired().HasMaxLength(16);
      this.Property(t => t.Sentido).HasColumnName("Sentido").HasMaxLength(6);

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Linha)
          .WithMany(f => f.ItinerariosDistinct).HasForeignKey(k => k.LinhaId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.Atendimento)
          .WithMany(f => f.ItinerariosDistinct).HasForeignKey(k => k.AtendimentoId).WillCascadeOnDelete(false);
    }
  }
}
