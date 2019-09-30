using System.Collections.Generic;

namespace Dto.Models {
  public class Via {
    public Via() {
      this.ItAtendimentos = new HashSet<ItAtendimento>();
      this.Itinerarios = new HashSet<Itinerario>();
      this.ItTroncos = new HashSet<ItTronco>();
      this.Abastecimentos = new HashSet<AAbastece>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }

    // Navigation Properties
    public virtual ICollection<ItAtendimento> ItAtendimentos { get; set; }
    public virtual ICollection<Itinerario> Itinerarios { get; set; }
    public virtual ICollection<ItTronco> ItTroncos { get; set; }
    public virtual ICollection<AAbastece> Abastecimentos { get; set; }
  }
}
