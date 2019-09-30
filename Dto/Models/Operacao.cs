using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Operacao {
    public Operacao() {
      this.Linhas = new HashSet<Linha>();
      this.Pesquisas = new HashSet<Pesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int OperLinhaId { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Empresa Empresa { get; set; }
    public virtual OperLinha OperLinha { get; set; }

    public virtual ICollection<Linha> Linhas { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
  }
}
