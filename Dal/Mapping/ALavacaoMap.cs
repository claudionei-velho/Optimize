using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ALavacaoMap : EntityTypeConfiguration<ALavacao> {
    public ALavacaoMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ALavacao", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Maquinas).HasColumnName("Maquinas");
      this.Property(x => x.Aguam3).HasColumnName("Aguam3").IsRequired().HasPrecision(18, 3);

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Lavacoes).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
