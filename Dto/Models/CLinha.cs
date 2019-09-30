using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class CLinha {
    public CLinha() {
      this.Linhas = new HashSet<Linha>();
      this.Pesquisas = new HashSet<Pesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int ClassLinhaId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual ClassLinha ClassLinha { get; set; }

    public virtual ICollection<Linha> Linhas { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
  }
}
