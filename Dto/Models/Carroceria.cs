using System;

namespace Dto.Models {
  public class Carroceria {
    public int VeiculoId { get; set; }
    public string Fabricante { get; set; }
    public string Modelo { get; set; }
    public string Referencia { get; set; }
    public int? Ano { get; set; }
    public DateTime? Aquisicao { get; set; }
    public string Fornecedor { get; set; }
    public string NotaFiscal { get; set; }
    public decimal? Valor { get; set; }
    public string ChaveNfe { get; set; }
    public DateTime? Encarrocamento { get; set; }
    public string QuemEncarroca { get; set; }
    public string NotaEncarroca { get; set; }
    public decimal? ValorEncarroca { get; set; }
    public byte Portas { get; set; }
    public byte? Assentos { get; set; }
    public byte? Capacidade { get; set; }
    public string Piso { get; set; }
    public bool EscapeV { get; set; }
    public bool EscapeH { get; set; }
    public int? Catraca { get; set; }
    public int PortaIn { get; set; }
    public bool SaidaFrente { get; set; }
    public bool SaidaMeio { get; set; }
    public bool SaidaTras { get; set; }

    // Navigation Properties
    public virtual Veiculo Veiculo { get; set; }
  }
}
