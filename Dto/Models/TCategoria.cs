using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class TCategoria {
    public TCategoria() {
      this.Ofertas = new HashSet<Oferta>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Denominacao { get; set; }
    public bool Gratuidade { get; set; }
    public decimal? Rateio { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<Oferta> Ofertas { get; set; }
  }
}
