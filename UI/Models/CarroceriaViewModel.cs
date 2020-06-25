using System;
using System.ComponentModel.DataAnnotations;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class CarroceriaViewModel {
    [Key]
    [Display(Name = "VeiculoId", ResourceType = typeof(Properties.Resources))]
    public int VeiculoId { get; set; }

    [Display(Name = "Fabricante", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Fabricante { get; set; }

    [Display(Name = "Modelo", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Modelo { get; set; }

    [Display(Name = "Referencia", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Referencia { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    public int? Ano { get; set; }

    [Display(Name = "Aquisicao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Aquisicao { get; set; }

    [Display(Name = "Fornecedor", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Fornecedor { get; set; }

    [Display(Name = "NotaFiscal", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string NotaFiscal { get; set; }

    [Display(Name = "Valor", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? Valor { get; set; }

    [Display(Name = "ChaveNfe", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChaveNfe { get; set; }

    [Display(Name = "Encarrocamento", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Encarrocamento { get; set; }

    [Display(Name = "QuemEncarroca", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string QuemEncarroca { get; set; }

    [Display(Name = "NotaEncarroca", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string NotaEncarroca { get; set; }

    [Display(Name = "ValorEncarroca", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? ValorEncarroca { get; set; }

    [Display(Name = "Portas", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PortasError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public byte Portas { get; set; }

    [Display(Name = "Assentos", ResourceType = typeof(Properties.Resources))]
    public byte? Assentos { get; set; }

    [Display(Name = "Capacidade", ResourceType = typeof(Properties.Resources))]
    public byte? Capacidade { get; set; }

    [Display(Name = "Piso", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Piso { get; set; }

    [Display(Name = "EscapeV", ResourceType = typeof(Properties.Resources))]
    public bool EscapeV { get; set; }

    [Display(Name = "EscapeH", ResourceType = typeof(Properties.Resources))]
    public bool EscapeH { get; set; }

    [Display(Name = "Catraca", ResourceType = typeof(Properties.Resources))]
    public int? Catraca { get; set; }

    public string CatracaCap {
      get {
        return Posicao.Items[this.Catraca ?? 0];
      }
    }

    [Display(Name = "PortaIn", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PortaInError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PortaIn { get; set; }

    public string PortaInCap {
      get {
        return Posicao.Items[this.PortaIn];
      }
    }

    [Display(Name = "SaidaFrente", ResourceType = typeof(Properties.Resources))]
    public bool SaidaFrente { get; set; }

    [Display(Name = "SaidaMeio", ResourceType = typeof(Properties.Resources))]
    public bool SaidaMeio { get; set; }

    [Display(Name = "SaidaTras", ResourceType = typeof(Properties.Resources))]
    public bool SaidaTras { get; set; }

    // Navigation Properties
    public virtual Veiculo Veiculo { get; set; }
  }
}
