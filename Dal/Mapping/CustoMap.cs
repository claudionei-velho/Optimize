using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CustoMap : EntityTypeConfiguration<Custo> {
    public CustoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Custos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Referencia).HasColumnName("Referencia");
      this.Property(t => t.Fixo).HasColumnName("Fixo").HasPrecision(38, 4);
      this.Property(t => t.Variavel).HasColumnName("Variavel").HasPrecision(38, 4);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Custos).HasForeignKey(d => d.EmpresaId).WillCascadeOnDelete(false);
    }
  }
}
