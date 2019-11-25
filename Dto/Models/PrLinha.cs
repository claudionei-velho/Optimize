using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class PrLinha {
    public PrLinha() {
      this.Horarios = new HashSet<Horario>();
      this.Viagens = new HashSet<Viagem>();
      this.TotalViagens = new HashSet<TotalViagem>();
      this.Dimensionamentos = new HashSet<Dimensionamento>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int PeriodoId { get; set; }
    public int DiaId { get; set; }
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

    public int? CicloAB { get; set; }
    public int? CicloBA { get; set; }

    [NotMapped]
    public int Ciclo {
      get {
        return this.CicloAB.Value + this.CicloBA.Value;
      }
    }

    public int? CVeiculoId { get; set; }
    public int? OcupacaoId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual EPeriodo EPeriodo { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual Ocupacao Ocupacao { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; }
    public virtual ICollection<Viagem> Viagens { get; set; }
    public virtual ICollection<TotalViagem> TotalViagens { get; set; }
    public virtual ICollection<Dimensionamento> Dimensionamentos { get; set; }
  }
}
