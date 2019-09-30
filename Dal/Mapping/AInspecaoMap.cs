using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AInspecaoMap : EntityTypeConfiguration<AInspecao> {
    public AInspecaoMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AInspecao", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Rampas).HasColumnName("Rampas").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Inspecoes).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
