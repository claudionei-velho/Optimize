using System.Collections.Generic;

namespace Dto.Models {
  public class Municipio {
    public Municipio() {
      this.Bacias = new HashSet<Bacia>();
      this.Empresas = new HashSet<Empresa>();
    }

    public int Id { get; set; }
    public int UfId { get; set; }
    public string Nome { get; set; }
    public string Estado { get; set; }

    // Navigation Properties
    public virtual Uf Uf { get; set; }

    public virtual ICollection<Bacia> Bacias { get; set; }
    public virtual ICollection<Empresa> Empresas { get; set; }
  }
}
