using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CustoCoMap : EntityTypeConfiguration<CustoCo> {
    public CustoCoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("CustosEmpresa", "cst");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.Ano).HasColumnName("Ano").IsRequired();
      this.Property(t => t.Mes).HasColumnName("Mes").IsRequired();
      this.Property(t => t.RubricaId).HasColumnName("RubricaId").IsRequired();
      this.Property(t => t.SubtotalId).HasColumnName("SubtotalId");
      this.Property(t => t.TotalId).HasColumnName("TotalId");
      this.Property(t => t.Percurso).HasColumnName("Percurso").HasPrecision(18, 3);
      this.Property(t => t.Coeficiente).HasColumnName("Coeficiente").HasPrecision(18, 6);
      this.Property(t => t.Custo).HasColumnName("Custo").HasColumnType("money");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Rubrica)
          .WithMany(f => f.CoRubricas).HasForeignKey(k => k.RubricaId)
          .WillCascadeOnDelete(false);
    }
  }
}
