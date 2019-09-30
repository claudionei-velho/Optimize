using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Tronco {
    public Tronco() {
      this.ItTroncos = new HashSet<ItTronco>();
      this.PtTroncos = new HashSet<PtTronco>();
      this.LnTroncos = new HashSet<LnTronco>();
      this.Pesquisas = new HashSet<Pesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public decimal? ExtensaoAB { get; set; }
    public decimal? ExtensaoBA { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<ItTronco> ItTroncos { get; set; }
    public virtual ICollection<PtTronco> PtTroncos { get; set; }
    public virtual ICollection<LnTronco> LnTroncos { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
  }
}
