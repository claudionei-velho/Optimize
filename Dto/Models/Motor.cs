using System.Collections.Generic;

namespace Dto.Models {
  public class Motor {
    public Motor() {
      this.Chassis = new HashSet<Chassi>();
      this.SpecVeiculos = new HashSet<SpecVeiculo>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Classificacao { get; set; }
    public string Descricao { get; set; }

    public virtual ICollection<Chassi> Chassis { get; set; }
    public virtual ICollection<SpecVeiculo> SpecVeiculos { get; set; }
  }
}
