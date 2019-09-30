using System.Collections.Generic;

namespace Dto.Models {
  public class FxEtaria {
    public FxEtaria() {
      this.FrotaEtarias = new HashSet<FrotaEtaria>();
      this.SpecVeiculos = new HashSet<SpecVeiculo>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public int Minimo { get; set; }
    public int Maximo { get; set; }

    // Navigation Properties
    public virtual ICollection<FrotaEtaria> FrotaEtarias { get; set; }
    public virtual ICollection<SpecVeiculo> SpecVeiculos { get; set; }
  }
}
