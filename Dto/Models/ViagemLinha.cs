using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class ViagemLinha {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public int DiaId { get; set; }
    public string Prefixo { get; set; }

    [NotMapped]
    public decimal? Extensao {
      get {
        try {
          return (this.PercursoAno ?? 0) / this.ViagensAno.Value;
        }
        catch (DivideByZeroException) {
          throw;
        }
      }
    }

    [NotMapped]
    public int? ViagensSemana {
      get {
        try {
          return this.ViagensAno.Value / CustomCalendar.WeeksPerYear;
        }
        catch (DivideByZeroException) {
          throw;
        }
      }
    }

    [NotMapped]
    public decimal? PercursoSemana {
      get {
        try {
          return this.PercursoAno.Value / CustomCalendar.WeeksPerYear;
        }
        catch (DivideByZeroException) {
          throw;
        }
      }
    }

    [NotMapped]
    public int? ViagensMes {
      get {
        try {
          return this.ViagensAno.Value / CustomCalendar.MonthsPerYear;
        }
        catch (DivideByZeroException) {
          throw;
        }
      }
    }

    [NotMapped]
    public decimal? PercursoMes {
      get {
        try {
          return this.PercursoAno.Value / CustomCalendar.MonthsPerYear;
        }
        catch (DivideByZeroException) {
          throw;
        }
      }
    }

    public int? ViagensAno { get; set; }
    public decimal? PercursoAno { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
  }
}
