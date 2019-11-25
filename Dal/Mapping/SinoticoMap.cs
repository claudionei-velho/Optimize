using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class SinoticoMap : EntityTypeConfiguration<Sinotico> {
    public SinoticoMap() {
      // Primary Key
      this.HasKey(t => new { t.PesquisaId, t.LinhaId, t.DiaId, t.SinoticoId });

      // Table, Properties & Column Mappings
      this.ToTable("Sinotico", "opc");
      this.Property(t => t.PesquisaId).HasColumnName("PesquisaId");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.SinoticoId).HasColumnName("SinoticoId");

      // Relationships
      this.HasRequired(t => t.Pesquisa)
          .WithMany(t => t.Sinoticos).HasForeignKey(d => d.PesquisaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Sinoticos).HasForeignKey(d => d.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.ISinotico)
          .WithMany(t => t.Sinoticos).HasForeignKey(d => d.SinoticoId)
          .WillCascadeOnDelete(false);
    }
  }
}
