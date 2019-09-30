using System.Collections.Generic;

namespace Dto.Models {
  public class CVeiculoAtt {
    public CVeiculoAtt() {
      this.VeiculosAtt = new HashSet<VeiculoAtt>();
    }

    public int Id { get; set; }
    public string Caracteristica { get; set; }
    public string Unidade { get; set; }
    public bool Variavel { get; set; }

    public virtual ICollection<VeiculoAtt> VeiculosAtt { get; set; }
  }
}
