using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Horario {
    public Horario() {
      this.Viagens = new HashSet<Viagem>();
      this.VetoresH = new HashSet<VetorH>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public int DiaId { get; set; }
    public int? Item { get; set; }
    public string Sentido { get; set; }
    public TimeSpan Inicio { get; set; }
    public int? PeriodoId { get; set; }
    public decimal? Extensao { get; set; }    
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Atendimento Atendimento { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }

    public virtual ICollection<Viagem> Viagens { get; set; }
    public virtual ICollection<VetorH> VetoresH { get; set; }
  }
}
