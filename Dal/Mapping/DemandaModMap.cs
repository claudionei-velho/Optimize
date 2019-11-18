using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DemandaModMap : EntityTypeConfiguration<DemandaMod> {
    public DemandaModMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.Ano, t.Mes });

      // Table, Properties & Column Mappings
      this.ToTable("DemandaMod", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Ano).HasColumnName("Ano").IsRequired();
      this.Property(t => t.Mes).HasColumnName("Mes").IsRequired();
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Equivalente).HasColumnName("Equivalente");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.DemandasMod).HasForeignKey(k => k.LinhaId)
          .WillCascadeOnDelete(false);
    }
  }
}
