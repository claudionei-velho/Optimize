using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class PeriodoTipico {
    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int DiaId { get; set; }
    public int PeriodoId { get; set; }
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

    public int QtdViagens { get; set; }
    public int? CicloAB { get; set; }
    public int? CicloBA { get; set; }

    [NotMapped]
    public int Ciclo {
      get {
        return (this.CicloAB ?? 0) + (this.CicloBA ?? 0);
      }
    }

    [NotMapped]
    public int? MaxVeiculos {
      get {        
        int result;
        try {
          int tempo = ((this.CicloAB ?? 0) > (this.CicloBA ?? 0)) ? this.CicloAB.Value : this.CicloBA.Value;
          result = (int)Math.Ceiling(tempo * (this.QtdViagens / (decimal)this.Duracao));
        }
        catch (DivideByZeroException) {
          result = this.QtdViagens;
        }
        return (result > this.QtdViagens) ? this.QtdViagens : result;
      }
    }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual EPeriodo EPeriodo { get; set; }
  }
}
