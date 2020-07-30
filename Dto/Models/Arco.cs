using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Arco {
    public Arco() {
      this.ArcosV = new HashSet<ArcoV>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int DiaId { get; set; }
    public TimeSpan Inicio { get; set; }
    public int? PInicioId { get; set; }
    public TimeSpan Termino { get; set; }
    public int? PTerminoId { get; set; }
    public int? Duracao { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Ponto PInicio { get; set; }
    public virtual Ponto PTermino { get; set; }

    public virtual ICollection<ArcoV> ArcosV { get; set; }
  }
}
