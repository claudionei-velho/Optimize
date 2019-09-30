using System.Collections.Generic;

namespace Dto.Models {
  public class OperLinha {
    public OperLinha() {
      this.Operacoes = new HashSet<Operacao>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }

    // Navigation Properties
    public virtual ICollection<Operacao> Operacoes { get; set; }
  }
}
