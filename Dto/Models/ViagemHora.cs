using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class ViagemHora {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int Hora { get; set; }

    [NotMapped]
    public string HoraCat {
      get {
        return $"{this.Hora}:00 às {this.Hora}:59";
      }
    }

    public int? UteisAB { get; set; }
    public int? UteisBA { get; set; }
    public int? SabadosAB { get; set; }
    public int? SabadosBA { get; set; }
    public int? DomingosAB { get; set; }
    public int? DomingosBA { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
  }
}
