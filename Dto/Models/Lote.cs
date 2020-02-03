using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Lote {
    public Lote() {
      this.Linhas = new HashSet<Linha>();
    }

    public int Id { get; set; }
    public int BaciaId { get; set; }
    public string Denominacao { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Bacia Bacia { get; set; }

    public virtual ICollection<Linha> Linhas { get; set; }
  }
}
