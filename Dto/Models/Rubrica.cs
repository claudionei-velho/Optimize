using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Rubrica {
    public Rubrica() {
      this.CoRubricas = new HashSet<CustoCo>();
      this.LnRubricas = new HashSet<CustoLn>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public bool Sintetica { get; set; }
    public int? SinteticaId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual ICollection<CustoCo> CoRubricas { get; set; }
    public virtual ICollection<CustoLn> LnRubricas { get; set; }
  }
}
