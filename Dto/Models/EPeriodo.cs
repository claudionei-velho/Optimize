using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class EPeriodo {
    public EPeriodo() {
      this.PrLinhas = new HashSet<PrLinha>();

      // Reports (Database Views)
      this.PeriodosTipicos = new HashSet<PeriodoTipico>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int PeriodoId { get; set; }
    public string Denominacao { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Empresa Empresa { get; set; }
    public virtual Periodo Periodo { get; set; }

    // Navigation Properties
    public virtual ICollection<PrLinha> PrLinhas { get; set; }

    // Reports (Database Views)
    public virtual ICollection<PeriodoTipico> PeriodosTipicos { get; set; }
  }
}
