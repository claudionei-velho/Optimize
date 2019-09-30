using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FViagemMap : EntityTypeConfiguration<FViagem> {
    public FViagemMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Acumulado)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("FViagens", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.ViagemId).HasColumnName("ViagemId");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.Embarques).HasColumnName("Embarques");
      this.Property(t => t.Desembarques).HasColumnName("Desembarques");
      this.Property(t => t.Acumulado).HasColumnName("Acumulado");

      // Relationships
      this.HasRequired(t => t.PtLinha)
          .WithMany(t => t.FViagens).HasForeignKey(d => d.PontoId);

      this.HasRequired(t => t.Viagem)
          .WithMany(t => t.FViagens).HasForeignKey(d => d.ViagemId);
    }
  }
}
