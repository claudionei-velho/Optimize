using System.Collections.Generic;

namespace Dto.Models {
  public class CVeiculo {
    public CVeiculo() {
      this.ECVeiculos = new HashSet<ECVeiculo>();
      this.PrLinhas = new HashSet<PrLinha>();
      this.Veiculos = new HashSet<Veiculo>();
      this.VeiculosAtt = new HashSet<VeiculoAtt>();
      this.SpecVeiculos = new HashSet<SpecVeiculo>();
    }

    public int Id { get; set; }
    public string Categoria { get; set; }
    public string Classe { get; set; }
    public int? Minimo { get; set; }
    public int? Maximo { get; set; }

    // Navigation Properties
    public virtual ICollection<ECVeiculo> ECVeiculos { get; set; }
    public virtual ICollection<PrLinha> PrLinhas { get; set; }
    public virtual ICollection<Veiculo> Veiculos { get; set; }
    public virtual ICollection<VeiculoAtt> VeiculosAtt { get; set; }
    public virtual ICollection<SpecVeiculo> SpecVeiculos { get; set; }
  }
}
