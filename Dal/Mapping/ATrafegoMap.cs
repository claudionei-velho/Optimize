using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ATrafegoMap : EntityTypeConfiguration<ATrafego> {
    public ATrafegoMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ATrafego", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.InstalacaoId).HasColumnName("InstalacaoId").IsRequired();
      this.Property(x => x.Plantao).HasColumnName("Plantao");
      this.Property(x => x.Reserva).HasColumnName("Reserva");
      this.Property(x => x.Equipamentos).HasColumnName("Equipamentos");
      this.Property(x => x.Mobiliario).HasColumnName("Mobiliario");

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.EInstalacao)
          .WithMany(b => b.Trafegos).HasForeignKey(c => c.InstalacaoId).WillCascadeOnDelete(false);
    }
  }
}
