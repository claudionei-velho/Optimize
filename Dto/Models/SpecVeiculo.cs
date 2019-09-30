using System;

namespace Dto.Models {
  public class SpecVeiculo {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Numero { get; set; }
    public string Cor { get; set; }
    public int Classe { get; set; }
    public int? Categoria { get; set; }
    public string Placa { get; set; }
    public string Renavam { get; set; }
    public string Antt { get; set; }
    public DateTime? Inicio { get; set; }
    public string ChassiFabricante { get; set; }
    public string ChassiModelo { get; set; }
    public string ChassiNo { get; set; }
    public int? ChassiAno { get; set; }
    public DateTime? ChassiAquisicao { get; set; }
    public string ChassiFornecedor { get; set; }
    public string ChassiNota { get; set; }
    public decimal? ChassiValor { get; set; }
    public string ChassiChaveNfe { get; set; }
    public int? MotorId { get; set; }
    public string Potencia { get; set; }
    public int? PosMotor { get; set; }
    public byte? EixosFrente { get; set; }
    public byte? EixosTras { get; set; }
    public string PneusFrente { get; set; }
    public string PneusTras { get; set; }
    public int? TransmiteId { get; set; }
    public int? DirecaoId { get; set; }
    public string CarroceriaFabricante { get; set; }
    public string CarroceriaModelo { get; set; }
    public string Referencia { get; set; }
    public int? CarroceriaAno { get; set; }
    public DateTime? CarroceriaAquisicao { get; set; }
    public string CarroceriaFornecedor { get; set; }
    public string CarroceriaNota { get; set; }
    public decimal? CarroceriaValor { get; set; }
    public string CarroceriaChaveNfe { get; set; }
    public DateTime? Encarrocamento { get; set; }
    public string QuemEncarroca { get; set; }
    public string NotaEncarroca { get; set; }
    public decimal? ValorEncarroca { get; set; }
    public byte? Portas { get; set; }
    public byte? Assentos { get; set; }
    public byte? Capacidade { get; set; }
    public string Piso { get; set; }
    public bool EscapeV { get; set; }
    public bool EscapeH { get; set; }
    public int? Catraca { get; set; }
    public int? PortaIn { get; set; }
    public bool SaidaFrente { get; set; }
    public bool SaidaMeio { get; set; }
    public bool SaidaTras { get; set; }
    public int? EtariaId { get; set; }
    public int? Idade { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual Empresa Empresa { get; set; }
    public virtual Motor Motor { get; set; }
    public virtual FxEtaria FxEtaria { get; set; }
  }
}
