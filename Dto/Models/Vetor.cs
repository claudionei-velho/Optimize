using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Vetor {
    public Vetor() {
      this.VetoresH = new HashSet<VetorH>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int DiaId { get; set; }
    public TimeSpan Inicio { get; set; }
    public int? PInicioId { get; set; }
    public TimeSpan Termino { get; set; }
    public int? PTerminoId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Ponto PInicio { get; set; }
    public virtual Ponto PTermino { get; set; }

    public virtual ICollection<VetorH> VetoresH { get; set; }
  }
}
