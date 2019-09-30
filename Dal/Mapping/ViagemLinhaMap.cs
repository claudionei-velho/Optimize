using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ViagemLinhaMap : EntityTypeConfiguration<ViagemLinha> {
    public ViagemLinhaMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.DiaId, t.Prefixo });

      // Table, Properties & Column Mappings
      this.ToTable("ViagensLinha", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.DiaId).HasColumnName("DiaId").IsRequired();
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").IsRequired().HasMaxLength(16);
      this.Property(t => t.ViagensAno).HasColumnName("ViagensAno");
      this.Property(t => t.PercursoAno).HasColumnName("PercursoAno");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.ViagensLinha).HasForeignKey(k => k.LinhaId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.Atendimento)
          .WithMany(f => f.ViagensLinha).HasForeignKey(k => k.AtendimentoId).WillCascadeOnDelete(false);
    }
  }
}
