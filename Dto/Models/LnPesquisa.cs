using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class LnPesquisa {
    public LnPesquisa() {
      this.Viagens = new HashSet<Viagem>();
    }

    public int Id { get; set; }
    public int PesquisaId { get; set; }
    public int LinhaId { get; set; }
    public bool Uteis { get; set; }
    public bool Sabados { get; set; }
    public bool Domingos { get; set; }
    public string Responsavel { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Linha Linha { get; set; }
    public virtual Pesquisa Pesquisa { get; set; }

    public virtual ICollection<Viagem> Viagens { get; set; }
  }
}
