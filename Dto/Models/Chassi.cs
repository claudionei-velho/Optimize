using System;

namespace Dto.Models {
  public class Chassi {
    public int VeiculoId { get; set; }
    public string Fabricante { get; set; }
    public string Modelo { get; set; }
    public string ChassiNo { get; set; }
    public int? Ano { get; set; }
    public DateTime? Aquisicao { get; set; }
    public string Fornecedor { get; set; }
    public string NotaFiscal { get; set; }
    public decimal? Valor { get; set; }
    public string ChaveNfe { get; set; }
    public int? MotorId { get; set; }
    public string Potencia { get; set; }
    public int? PosMotor { get; set; }
    public byte EixosFrente { get; set; }
    public byte EixosTras { get; set; }
    public string PneusFrente { get; set; }
    public string PneusTras { get; set; }
    public int? TransmiteId { get; set; }
    public int? DirecaoId { get; set; }

    // Navigation Properties
    public virtual Motor Motor { get; set; }
    public virtual Veiculo Veiculo { get; set; }
  }
}
