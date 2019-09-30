using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class MapaLinhaMap : EntityTypeConfiguration<MapaLinha> {
    public MapaLinhaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("MapasLinha", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Sentido).HasColumnName("Sentido").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.Descricao).HasColumnName("Descricao").HasMaxLength(64);
      this.Property(t => t.Arquivo).HasColumnName("Arquivo").IsRequired().HasMaxLength(256);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.MapasLinha).HasForeignKey(k => k.LinhaId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.Atendimento)
          .WithMany(f => f.MapasLinha).HasForeignKey(k => k.AtendimentoId).WillCascadeOnDelete(false);
    }
  }
}
