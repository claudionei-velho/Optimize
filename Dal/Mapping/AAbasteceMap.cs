using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AAbasteceMap : EntityTypeConfiguration<AAbastece> {
    public AAbasteceMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AAbastece", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.PavimentoId).HasColumnName("PavimentoId").IsRequired();
      this.Property(x => x.Bombas).HasColumnName("Bombas").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Abastecimentos).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
