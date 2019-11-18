using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class OperacionalMap : EntityTypeConfiguration<Operacional> {
    public OperacionalMap() {
      // Primary Key
      this.HasKey(t => new { t.EmpresaId, t.LinhaId, t.Prefixo, t.Sentido });

      // Table, Properties & Column Mappings
      this.ToTable("Operacional", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").IsRequired().HasMaxLength(16);
      this.Property(t => t.Denominacao).HasColumnName("Denominacao").IsRequired().HasMaxLength(128);
      this.Property(t => t.Sentido).HasColumnName("Sentido").HasMaxLength(6);
      this.Property(t => t.DiaOperacao).HasColumnName("DiaOperacao").HasMaxLength(32);
      this.Property(t => t.Funcao).HasColumnName("Funcao").HasMaxLength(64);
      this.Property(t => t.Extensao).HasColumnName("Extensao").HasPrecision(18, 3);
      this.Property(t => t.ViagensUtil).HasColumnName("ViagensUtil");
      this.Property(t => t.PercursoUtil).HasColumnName("PercursoUtil").HasPrecision(29, 3);
      this.Property(t => t.InicioUtil).HasColumnName("InicioUtil");
      this.Property(t => t.ViagensSab).HasColumnName("ViagensSab");
      this.Property(t => t.PercursoSab).HasColumnName("PercursoSab").HasPrecision(29, 3);
      this.Property(t => t.InicioSab).HasColumnName("InicioSab");
      this.Property(t => t.ViagensDom).HasColumnName("ViagensDom");
      this.Property(t => t.PercursoDom).HasColumnName("PercursoDom").HasPrecision(29, 3);
      this.Property(t => t.InicioDom).HasColumnName("InicioDom");

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Linha)
          .WithMany(f => f.Operacionais).HasForeignKey(k => k.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasOptional(t => t.Atendimento)
          .WithMany(f => f.Operacionais).HasForeignKey(k => k.AtendimentoId)
          .WillCascadeOnDelete(false);
    }
  }
}
