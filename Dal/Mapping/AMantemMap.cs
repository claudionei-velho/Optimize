using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AMantenMap : EntityTypeConfiguration<AMantem> {
    public AMantenMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AMantem", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Area).HasColumnName("Area").HasPrecision(18, 3);
      this.Property(x => x.PontosAr).HasColumnName("PontosAr");
      this.Property(x => x.Eletricidade).HasColumnName("Eletricidade").IsRequired();
      this.Property(x => x.Elevadores).HasColumnName("Elevadores");

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Manutencoes).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
