using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class TotalViagem {
    public int LinhaId { get; set; }
    public int DiaId { get; set; }
    public int PeriodoId { get; set; }
    public string Sentido { get; set; }    
    public TimeSpan Inicio { get; set; }
    public TimeSpan Termino { get; set; }

    [NotMapped]
    public int Duracao {
      get {
        return ((int)this.Termino.Subtract(this.Inicio).TotalMinutes < 0) ?
          1440 + (int)this.Termino.Subtract(this.Inicio).TotalMinutes : 
            (int)this.Termino.Subtract(this.Inicio).TotalMinutes;
      }
    }

    public int Ciclo { get; set; }
    public int QtdViagens { get; set; }
    public int? QtdAtendimentos { get; set; }

    [NotMapped]
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

    [NotMapped]
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

    public decimal? KmDia { get; set; }
    public decimal? KmSemana { get; set; }
    public decimal? KmMes { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }
  }
}
