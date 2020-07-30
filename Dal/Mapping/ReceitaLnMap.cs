using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ReceitaLnMap : EntityTypeConfiguration<ReceitaLn> {
    public ReceitaLnMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.Ano, t.Mes });

      // Table, Properties & Column Mappings
      this.ToTable("ReceitasLn", "cst");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Ano).HasColumnName("Ano").IsRequired();
      this.Property(t => t.Mes).HasColumnName("Mes").IsRequired();
      this.Property(t => t.Passageiros).HasColumnName("Passageiros").IsRequired();
      this.Property(t => t.Receita).HasColumnName("Receita").HasColumnType("money");
      this.Property(t => t.Aliquota).HasColumnName("Aliquota").HasPrecision(18, 6);
    }
  }
}
