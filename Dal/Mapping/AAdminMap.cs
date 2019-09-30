using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AAdminMap : EntityTypeConfiguration<AAdmin> {
    public AAdminMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AAdmin", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Requisitom2).HasColumnName("Requisitom2").HasPrecision(18, 3);
      this.Property(x => x.Disponivelm2).HasColumnName("Disponivelm2").IsRequired().HasPrecision(18, 3);

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Administracoes).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
