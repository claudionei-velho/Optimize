using System.Collections.Generic;

namespace Dto.Models {
  public class ClassLinha {
    public ClassLinha() {
      this.CLinhas = new HashSet<CLinha>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }

    // Navigation Properties
    public virtual ICollection<CLinha> CLinhas { get; set; }
  }
}
