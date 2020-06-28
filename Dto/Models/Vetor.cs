using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Vetor {
    public int Id { get; set; }
    public int DiaId { get; set; }
    // public int PInicioId { get; set; }
    // public TimeSpan Inicio { get; set; }
    // public int PTerminoId { get; set; }
    // public TimeSpan Termino { get; set; }
    public int? AnteriorId { get; set; }
    public int? ProximoId { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    //public virtual Ponto PInicio { get; set; }
    //public virtual Ponto PTermino { get; set; }
    public virtual Vetor Anterior { get; set; }
    public virtual Vetor Proximo { get; set; }

    public virtual ICollection<Vetor> Anteriores { get; set; }
    public virtual ICollection<Vetor> Proximos { get; set; }
  }
}
