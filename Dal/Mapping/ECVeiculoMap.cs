using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ECVeiculoMap : EntityTypeConfiguration<ECVeiculo> {
    public ECVeiculoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ECVeiculos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.ClasseId).HasColumnName("ClasseId").IsRequired();
      this.Property(t => t.Minimo).HasColumnName("Minimo");
      this.Property(t => t.Maximo).HasColumnName("Maximo");
      this.Property(t => t.Passageirom2).HasColumnName("Passageirom2").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Empresa)
          .WithMany(f => f.ECVeiculos).HasForeignKey(k => k.EmpresaId).WillCascadeOnDelete(false);

      this.HasRequired(p => p.CVeiculo)
          .WithMany(f => f.ECVeiculos).HasForeignKey(k => k.ClasseId).WillCascadeOnDelete(false);
    }
  }
}
