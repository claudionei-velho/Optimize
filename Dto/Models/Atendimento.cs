using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dto.Models {
  public class Atendimento {
    public Atendimento() {
      this.ItAtendimentos = new HashSet<ItAtendimento>();
      this.Horarios = new HashSet<Horario>();
      this.MapasLinha = new HashSet<MapaLinha>();
      this.PtAtendimentos = new HashSet<PtAtendimento>();
      this.Referencias = new HashSet<Referencia>();

      // Reports (Database Views)
      this.Operacionais = new HashSet<Operacional>();
      this.PlanOperacionais = new HashSet<PlanOperacional>();
      this.ViagensLinha = new HashSet<ViagemLinha>();
      this.ItinerariosDistinct = new HashSet<ItinerarioDistinct>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public bool Uteis { get; set; }
    public bool Sabados { get; set; }
    public bool Domingos { get; set; }
    public bool Escolar { get; set; }

    [NotMapped]
    public string DiasOp {
      get {
        char[] charsToTrim = { ' ', ';' };

        StringBuilder aux = new StringBuilder();
        if (this.Uteis) {
          aux.Append("Dias Úteis; ");
        }
        if (this.Sabados) {
          aux.Append("Sábados; ");
        }
        if (this.Domingos) {
          aux.Append("Domingos");
        }
        return aux.ToString().Trim(charsToTrim);
      }
    }

    public decimal? ExtensaoAB { get; set; }
    public decimal? ExtensaoBA { get; set; }

    [NotMapped]
    public decimal? Extensao {
      get {
        decimal? result = (this.ExtensaoAB ?? 0) + (this.ExtensaoBA ?? 0);
        return (result != 0m) ? result : null;
      }
    }

    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }

    public virtual ICollection<ItAtendimento> ItAtendimentos { get; set; }
    public virtual ICollection<Horario> Horarios { get; set; }
    public virtual ICollection<MapaLinha> MapasLinha { get; set; }
    public virtual ICollection<PtAtendimento> PtAtendimentos { get; set; }
    public virtual ICollection<Referencia> Referencias { get; set; }

    // Reports (Database Views)
    public virtual ICollection<Operacional> Operacionais { get; set; }
    public virtual ICollection<PlanOperacional> PlanOperacionais { get; set; }
    public virtual ICollection<ViagemLinha> ViagensLinha { get; set; }
    public virtual ICollection<ItinerarioDistinct> ItinerariosDistinct { get; set; }
  }
}
