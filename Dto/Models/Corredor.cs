using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Corredor {
    public Corredor() {
      this.LnCorredores = new HashSet<LnCorredor>();
      this.Pesquisas = new HashSet<Pesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public string Caracteristicas { get; set; }
    public string Municipio { get; set; }
    public string UfId { get; set; }
    public decimal? Extensao { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<LnCorredor> LnCorredores { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
  }
}
