using System.Collections.Generic;

namespace Dto.Models {
  public class ISinotico {
    public ISinotico() {
      this.Sinoticos = new HashSet<Sinotico>();
    }

    public int Id { get; set; }
    public string Classe { get; set; }
    public string Denominacao { get; set; }
    public string Unidade { get; set; }

    // Navigation Properties
    public virtual ICollection<Sinotico> Sinoticos { get; set; }
  }
}
