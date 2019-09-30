using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ALubrificaMap : EntityTypeConfiguration<ALubrifica> {
    public ALubrificaMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ALubrifica", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Lavacao).HasColumnName("Lavacao");
      this.Property(x => x.Ceramico).HasColumnName("Ceramico").IsRequired();
      this.Property(x => x.Protecao).HasColumnName("Protecao").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Lubrificacoes).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
