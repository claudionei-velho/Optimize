using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CarroceriaMap : EntityTypeConfiguration<Carroceria> {
    public CarroceriaMap() {
      // Primary Key
      this.HasKey(t => t.VeiculoId);

      // Table, Properties & Column Mappings
      this.ToTable("Carrocerias", "opc");
      this.Property(t => t.VeiculoId).HasColumnName("VeiculoId").IsRequired();
      this.Property(t => t.Fabricante).HasColumnName("Fabricante").HasMaxLength(64);
      this.Property(t => t.Modelo).HasColumnName("Modelo").HasMaxLength(64);
      this.Property(t => t.Referencia).HasColumnName("Referencia").HasMaxLength(32);
      this.Property(t => t.Ano).HasColumnName("Ano");
      this.Property(t => t.Aquisicao).HasColumnName("Aquisicao");
      this.Property(t => t.Fornecedor).HasColumnName("Fornecedor").HasMaxLength(64);
      this.Property(t => t.NotaFiscal).HasColumnName("NotaFiscal").HasMaxLength(16);
      this.Property(t => t.Valor).HasColumnName("Valor");
      this.Property(t => t.ChaveNfe).HasColumnName("ChaveNfe").HasMaxLength(64);
      this.Property(t => t.Encarrocamento).HasColumnName("Encarrocamento");
      this.Property(t => t.QuemEncarroca).HasColumnName("QuemEncarroca").HasMaxLength(64);
      this.Property(t => t.NotaEncarroca).HasColumnName("NotaEncarroca").HasMaxLength(16);
      this.Property(t => t.ValorEncarroca).HasColumnName("ValorEncarroca");
      this.Property(t => t.Portas).HasColumnName("Portas").IsRequired();
      this.Property(t => t.Assentos).HasColumnName("Assentos");
      this.Property(t => t.Capacidade).HasColumnName("Capacidade");
      this.Property(t => t.Piso).HasColumnName("Piso").HasMaxLength(32);
      this.Property(t => t.EscapeV).HasColumnName("EscapeV").IsRequired();
      this.Property(t => t.EscapeH).HasColumnName("EscapeH").IsRequired();
      this.Property(t => t.Catraca).HasColumnName("Catraca");
      this.Property(t => t.PortaIn).HasColumnName("PortaIn").IsRequired();
      this.Property(t => t.SaidaFrente).HasColumnName("SaidaFrente").IsRequired();
      this.Property(t => t.SaidaMeio).HasColumnName("SaidaMeio").IsRequired();
      this.Property(t => t.SaidaTras).HasColumnName("SaidaTras").IsRequired();

      // Relationships
      this.HasRequired(t => t.Veiculo)
          .WithOptional(t => t.Carroceria).WillCascadeOnDelete(false);
    }
  }
}
