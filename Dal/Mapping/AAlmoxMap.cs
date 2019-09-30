using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AAlmoxMap : EntityTypeConfiguration<AAlmox> {
    public AAlmoxMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AAlmox", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Area).HasColumnName("Area").HasPrecision(18, 3);
      this.Property(x => x.Especifico).HasColumnName("Especifico").IsRequired();
      this.Property(x => x.Estoque).HasColumnName("Estoque").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Almoxarifados).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
