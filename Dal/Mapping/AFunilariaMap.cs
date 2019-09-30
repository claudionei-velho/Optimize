using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AFunilariaMap : EntityTypeConfiguration<AFunilaria> {
    public AFunilariaMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("AFunilaria", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Area).HasColumnName("Area").HasPrecision(18, 3);
      this.Property(x => x.Isolada).HasColumnName("Isolada").IsRequired();
      this.Property(x => x.PPoluicao).HasColumnName("PPoluicao");
      this.Property(x => x.Exaustao).HasColumnName("Exaustao");

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Funilarias).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
