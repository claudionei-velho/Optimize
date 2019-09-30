using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class EDominio {
    public EDominio() {
      this.Linhas = new HashSet<Linha>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int DominioId { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Dominio Dominio { get; set; }
    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<Linha> Linhas { get; set; }
  }
}
