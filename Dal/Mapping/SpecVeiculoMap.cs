using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  public class SpecVeiculoMap : EntityTypeConfiguration<SpecVeiculo> {
    public SpecVeiculoMap() {
      // Primary Key
      this.HasKey(x => x.Id);

      // Table, Properties & Column Mappings
      this.ToTable("SpecVeiculos", "opc");
      this.Property(x => x.Id).HasColumnName("Id");
      this.Property(x => x.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(x => x.Numero).HasColumnName("Numero").IsRequired().HasMaxLength(16);
      this.Property(x => x.Cor).HasColumnName("Cor").IsRequired().HasMaxLength(32);
      this.Property(x => x.Classe).HasColumnName("Classe").IsRequired();
      this.Property(x => x.Categoria).HasColumnName("Categoria");
      this.Property(x => x.Placa).HasColumnName("Placa").IsRequired().HasMaxLength(16);
      this.Property(x => x.Renavam).HasColumnName("Renavam").IsRequired().HasMaxLength(16);
      this.Property(x => x.Antt).HasColumnName("Antt").HasMaxLength(16);
      this.Property(x => x.Inicio).HasColumnName("Inicio");
      this.Property(x => x.ChassiFabricante).HasColumnName("ChassiFabricante").HasMaxLength(64);
      this.Property(x => x.ChassiModelo).HasColumnName("ChassiModelo").HasMaxLength(64);
      this.Property(x => x.ChassiNo).HasColumnName("ChassiNo").HasMaxLength(32);
      this.Property(x => x.ChassiAno).HasColumnName("ChassiAno");
      this.Property(x => x.ChassiAquisicao).HasColumnName("ChassiAquisicao");
      this.Property(x => x.ChassiFornecedor).HasColumnName("ChassiFornecedor").HasMaxLength(64);
      this.Property(x => x.ChassiNota).HasColumnName("ChassiNota").HasMaxLength(16);
      this.Property(x => x.ChassiValor).HasColumnName("ChassiValor").HasPrecision(19, 4);
      this.Property(x => x.ChassiChaveNfe).HasColumnName("ChassiChaveNfe").HasMaxLength(64);
      this.Property(x => x.MotorId).HasColumnName("MotorId");
      this.Property(x => x.Potencia).HasColumnName("Potencia").HasMaxLength(32);
      this.Property(x => x.PosMotor).HasColumnName("PosMotor");
      this.Property(x => x.EixosFrente).HasColumnName("EixosFrente");
      this.Property(x => x.EixosTras).HasColumnName("EixosTras");
      this.Property(x => x.PneusFrente).HasColumnName("PneusFrente").HasMaxLength(16);
      this.Property(x => x.PneusTras).HasColumnName("PneusTras").HasMaxLength(16);
      this.Property(x => x.TransmiteId).HasColumnName("TransmiteId");
      this.Property(x => x.DirecaoId).HasColumnName("DirecaoId");
      this.Property(x => x.CarroceriaFabricante).HasColumnName("CarroceriaFabricante").HasMaxLength(64);
      this.Property(x => x.CarroceriaModelo).HasColumnName("CarroceriaModelo").HasMaxLength(64);
      this.Property(x => x.Referencia).HasColumnName("Referencia").HasMaxLength(32);
      this.Property(x => x.CarroceriaAno).HasColumnName("CarroceriaAno");
      this.Property(x => x.CarroceriaAquisicao).HasColumnName("CarroceriaAquisicao");
      this.Property(x => x.CarroceriaFornecedor).HasColumnName("CarroceriaFornecedor").HasMaxLength(64);
      this.Property(x => x.CarroceriaNota).HasColumnName("CarroceriaNota").HasMaxLength(16);
      this.Property(x => x.CarroceriaValor).HasColumnName("CarroceriaValor").HasPrecision(19, 4);
      this.Property(x => x.CarroceriaChaveNfe).HasColumnName("CarroceriaChaveNfe").HasMaxLength(64);
      this.Property(x => x.Encarrocamento).HasColumnName("Encarrocamento");
      this.Property(x => x.QuemEncarroca).HasColumnName("QuemEncarroca").HasMaxLength(64);
      this.Property(x => x.NotaEncarroca).HasColumnName("NotaEncarroca").HasMaxLength(16);
      this.Property(x => x.ValorEncarroca).HasColumnName("ValorEncarroca").HasPrecision(19, 4);
      this.Property(x => x.Portas).HasColumnName("Portas");
      this.Property(x => x.Assentos).HasColumnName("Assentos");
      this.Property(x => x.Capacidade).HasColumnName("Capacidade");
      this.Property(x => x.Piso).HasColumnName("Piso").HasMaxLength(32);
      this.Property(x => x.EscapeV).HasColumnName("EscapeV");
      this.Property(x => x.EscapeH).HasColumnName("EscapeH");
      this.Property(x => x.Catraca).HasColumnName("Catraca");
      this.Property(x => x.PortaIn).HasColumnName("PortaIn");
      this.Property(x => x.SaidaFrente).HasColumnName("SaidaFrente");
      this.Property(x => x.SaidaMeio).HasColumnName("SaidaMeio");
      this.Property(x => x.SaidaTras).HasColumnName("SaidaTras");
      this.Property(x => x.EtariaId).HasColumnName("EtariaId");
      this.Property(x => x.Idade).HasColumnName("Idade");
      this.Property(x => x.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(a => a.Empresa)
          .WithMany(b => b.SpecVeiculos).HasForeignKey(c => c.EmpresaId).WillCascadeOnDelete(false);

      this.HasRequired(a => a.CVeiculo)
          .WithMany(b => b.SpecVeiculos).HasForeignKey(c => c.Classe).WillCascadeOnDelete(false);

      this.HasOptional(a => a.Motor)
          .WithMany(b => b.SpecVeiculos).HasForeignKey(c => c.MotorId).WillCascadeOnDelete(false);

      this.HasOptional(a => a.FxEtaria)
          .WithMany(b => b.SpecVeiculos).HasForeignKey(c => c.EtariaId).WillCascadeOnDelete(false);
    }
  }
}
