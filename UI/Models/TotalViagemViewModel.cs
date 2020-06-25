using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class TotalViagemViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return Workday.Data[DiaId];
      }
    }

    [Key, Column(Order = 2)]
    [Display(Name = "PeriodoId", ResourceType = typeof(Properties.Resources))]
    public int PeriodoId { get; set; }

    [Key, Column(Order = 3)]
    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

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

    [Display(Name = "Ciclo", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Ciclo { get; set; }

    [Display(Name = "TotalViagens", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int QtdViagens { get; set; }

    [Display(Name = "TotalAtendimentos", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? QtdAtendimentos { get; set; }

    [Display(Name = "Intervalo", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? IntervaloP {
      get {
        try {
          return this.Duracao / this.QtdViagens;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [Display(Name = "Veiculos", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? VeiculosP {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)this.Ciclo / this.IntervaloP ?? 0);
        }
        catch (DivideByZeroException) {
          result = null;
        }
        return (result.HasValue && (result > this.QtdViagens)) ? this.QtdViagens : result;
      }
    }

    [Display(Name = "KmDia", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmDia { get; set; }

    [Display(Name = "KmSemana", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmSemana { get; set; }

    [Display(Name = "KmMes", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0}")]
    public decimal? KmMes { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }
  }
}
