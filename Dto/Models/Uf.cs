using System.Collections.Generic;

namespace Dto.Models {
  public class Uf {
    public Uf() {
      this.Municipios = new HashSet<Municipio>();
    }

    public int Id { get; set; }
    public string Sigla { get; set; }
    public string Estado { get; set; }
    public string Capital { get; set; }
    public string Regiao { get; set; }

    // Navigation Properties
    public virtual ICollection<Municipio> Municipios { get; set; }
  }
}
