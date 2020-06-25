using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class DimensionamentoViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "PesquisaId", ResourceType = typeof(Properties.Resources))]
    public int PesquisaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Key, Column(Order = 2)]
    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return Workday.Items[DiaId];
      }
    }

    [Key, Column(Order = 3)]
    [Display(Name = "PeriodoId", ResourceType = typeof(Properties.Resources))]
    public int PeriodoId { get; set; }

    [Key, Column(Order = 4)]
    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

    [Display(Name = "QtdViagens", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int QtdViagens { get; set; }

    [Display(Name = "HoraInicio", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Display(Name = "HoraTermino", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Termino { get; set; }

    [Display(Name = "Duracao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Duracao {
      get {
        return ((int)Termino.Subtract(Inicio).TotalMinutes < 0) ?
          1440 + (int)Termino.Subtract(Inicio).TotalMinutes : (int)Termino.Subtract(Inicio).TotalMinutes;
      }
    }

    [Display(Name = "Ociosidade", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Ociosidade { get; set; }

    [Display(Name = "Passageiros", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Passageiros { get; set; }

    [Display(Name = "Ajustado", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Ajustado { get; set; }

    [Display(Name = "Critica", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Critica { get; set; }

    [Display(Name = "CriticaAjuste", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int CriticaAjuste { get; set; }

    [Display(Name = "Media", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Media {
      get {
        try {
          return (int)Math.Ceiling((decimal)Passageiros / QtdViagens);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "MediaAjuste", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? MediaAjuste {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / QtdViagens);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "Desvio", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Desvio { get; set; }

    [Display(Name = "DesvioAjuste", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int DesvioAjuste { get; set; }

    [Display(Name = "Intervalo", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Intervalo {
      get {
        try {
          return Duracao / QtdViagens;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "Fluxo", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.000}")]
    public decimal? Fluxo {
      get {
        try {
          return (decimal)Passageiros / Duracao;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "FluxoAjuste", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.000}")]
    public decimal? FluxoAjuste {
      get {
        try {
          return (decimal)Ajustado / Duracao;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "LotacaoE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int LotacaoE {
      get {
        return (MediaAjuste ?? 0) + DesvioAjuste;
      }
    }

    [Display(Name = "PrognosticoE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? PrognosticoE {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / LotacaoE);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "IntervaloE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? IntervaloE {
      get {
        try {
          return Duracao / PrognosticoE;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "LotacaoP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int LotacaoP { get; set; }

    [Display(Name = "PrognosticoP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? PrognosticoP {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / LotacaoP);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "IntervaloP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? IntervaloP {
      get {
        try {
          return Duracao / PrognosticoP;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "CicloAB", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? CicloAB { get; set; }

    [Display(Name = "CicloBA", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? CicloBA { get; set; }

    [Display(Name = "Veiculos", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Veiculos {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (Intervalo ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > this.QtdViagens) {
          result = this.QtdViagens;
        }
        return result;
      }
    }

    [Display(Name = "VeiculosE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? VeiculosE {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (IntervaloE ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > (PrognosticoE ?? 0)) {
          result = PrognosticoE ?? 0;
        }
        return result;
      }
    }

    [Display(Name = "VeiculosP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? VeiculosP {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (IntervaloP ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > (PrognosticoP ?? 0)) {
          result = PrognosticoP ?? 0;
        }
        return result;
      }
    }

    [Display(Name = "ExtensaoSentido", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}")]
    public decimal? Extensao { get; set; }

    [Display(Name = "KmTotal", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmTotal {
      get {
        return QtdViagens * Extensao;
      }
    }

    [Display(Name = "KmTotalE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmTotalE {
      get {
        return PrognosticoE * Extensao;
      }
    }

    [Display(Name = "KmTotalP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmTotalP {
      get {
        return PrognosticoP * Extensao;
      }
    }

    private int Tempo {
      get {
        return (this.CicloAB ?? 0) >= (this.CicloBA ?? 0) ? (this.CicloAB ?? 0) : (this.CicloBA ?? 0);
      }
    }

    // Navigation Properties
    public virtual Pesquisa Pesquisa { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }
  }
}
