using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class SinoticoMap : EntityTypeConfiguration<Sinotico> {
    public SinoticoMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.DiaId, t.Sentido, t.SinoticoId });

      // Properties
      this.Property(t => t.LinhaId)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      this.Property(t => t.DiaId)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      this.Property(t => t.Sentido)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      this.Property(t => t.SinoticoId)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      // Table & Column Mappings
      this.ToTable("Sinotico", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.SinoticoId).HasColumnName("SinoticoId");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Sinoticos).HasForeignKey(d => d.LinhaId);

      this.HasRequired(t => t.ISinotico)
          .WithMany(t => t.Sinoticos).HasForeignKey(d => d.SinoticoId);
    }
  }
}
