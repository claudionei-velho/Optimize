using System.Collections.Generic;

namespace Dto.Models {
  public class Ocupacao {
    public Ocupacao() {
      this.PrLinhas = new HashSet<PrLinha>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Nivel { get; set; }
    public decimal Densidade { get; set; }

    public virtual ICollection<PrLinha> PrLinhas { get; set; }
  }
}
