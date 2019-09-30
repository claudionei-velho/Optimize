using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ChassiMap : EntityTypeConfiguration<Chassi> {
    public ChassiMap() {
      // Primary Key
      this.HasKey(t => t.VeiculoId);

      // Table, Properties & Column Mappings
      this.ToTable("Chassis", "opc");
      this.Property(t => t.VeiculoId).HasColumnName("VeiculoId").IsRequired();
      this.Property(t => t.Fabricante).HasColumnName("Fabricante").HasMaxLength(64);
      this.Property(t => t.Modelo).HasColumnName("Modelo").HasMaxLength(64);
      this.Property(t => t.ChassiNo).HasColumnName("ChassiNo").HasMaxLength(32);
      this.Property(t => t.Ano).HasColumnName("Ano");
      this.Property(t => t.Aquisicao).HasColumnName("Aquisicao");
      this.Property(t => t.Fornecedor).HasColumnName("Fornecedor").HasMaxLength(64);
      this.Property(t => t.NotaFiscal).HasColumnName("NotaFiscal").HasMaxLength(16);
      this.Property(t => t.Valor).HasColumnName("Valor");
      this.Property(t => t.ChaveNfe).HasColumnName("ChaveNfe").HasMaxLength(64);
      this.Property(t => t.MotorId).HasColumnName("MotorId");
      this.Property(t => t.Potencia).HasColumnName("Potencia").HasMaxLength(32);
      this.Property(t => t.PosMotor).HasColumnName("PosMotor");
      this.Property(t => t.EixosFrente).HasColumnName("EixosFrente").IsRequired();
      this.Property(t => t.EixosTras).HasColumnName("EixosTras").IsRequired();
      this.Property(t => t.PneusFrente).HasColumnName("PneusFrente").HasMaxLength(16);
      this.Property(t => t.PneusTras).HasColumnName("PneusTras").HasMaxLength(16);
      this.Property(t => t.TransmiteId).HasColumnName("TransmiteId");
      this.Property(t => t.DirecaoId).HasColumnName("DirecaoId");

      // Relationships
      this.HasOptional(t => t.Motor)
          .WithMany(t => t.Chassis).HasForeignKey(d => d.MotorId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.Veiculo)
          .WithOptional(t => t.Chassi).WillCascadeOnDelete(false);
    }
  }
}
