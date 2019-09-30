using System.Collections.Generic;

namespace Dto.Models {
  public class Dominio {
    public Dominio() {
      this.EDominios = new HashSet<EDominio>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }

    public virtual ICollection<EDominio> EDominios { get; set; }
  }
}
