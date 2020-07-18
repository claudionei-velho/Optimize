using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ReferenciaMap : EntityTypeConfiguration<Referencia> {
    public ReferenciaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Referencias", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.Sentido).HasColumnName("Sentido").IsRequired()
          .IsFixedLength().HasMaxLength(2);

      this.Property(t => t.PInicioId).HasColumnName("PInicioId").IsRequired();
      this.Property(t => t.PTerminoId).HasColumnName("PTerminoId").IsRequired();
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Linha)
          .WithMany(f => f.Referencias).HasForeignKey(k => k.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(p => p.PInicio)
          .WithMany(f => f.PontosInicio).HasForeignKey(k => k.PInicioId)
          .WillCascadeOnDelete(false);

      this.HasRequired(p => p.PTermino)
          .WithMany(f => f.PontosTermino).HasForeignKey(k => k.PTerminoId)
          .WillCascadeOnDelete(false);

      this.HasOptional(p => p.Atendimento)
          .WithMany(f => f.Referencias).HasForeignKey(k => k.AtendimentoId);
    }
  }
}
