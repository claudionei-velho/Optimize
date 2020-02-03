using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Bacia {
    public Bacia() {
      this.Lotes = new HashSet<Lote>();
    }

    public int Id { get; set; }
    public int MunicipioId { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Municipio Municipio { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; }
  }
}
