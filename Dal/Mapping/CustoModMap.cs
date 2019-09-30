using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CustoModMap : EntityTypeConfiguration<CustoMod> {
    public CustoModMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table & Column Mappings
      this.ToTable("CustoMod", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Referencia).HasColumnName("Referencia");
      this.Property(t => t.Fixo).HasColumnName("Fixo");
      this.Property(t => t.Variavel).HasColumnName("Variavel");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.CustoMods).HasForeignKey(d => d.EmpresaId);
    }
  }
}
