using System.Collections.Generic;

namespace Dto.Models {
  public class Periodo {
    public Periodo() {
      this.EPeriodos = new HashSet<EPeriodo>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }

    public virtual ICollection<EPeriodo> EPeriodos { get; set; }
  }
}
