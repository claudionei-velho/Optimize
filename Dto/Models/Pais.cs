using System.Collections.Generic;

namespace Dto.Models {
  public class Pais {
    public Pais() {
      this.Empresas = new HashSet<Empresa>();
    }

    public string Id { get; set; }
    public string Nome { get; set; }
    public string Capital { get; set; }
    public string Continente { get; set; }

    public virtual ICollection<Empresa> Empresas { get; set; }
  }
}
