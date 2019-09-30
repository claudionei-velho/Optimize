using System.Collections.Generic;

namespace Dto.Models {
  public class FInstalacao {
    public FInstalacao() {
      this.EInstalacoes = new HashSet<EInstalacao>();
    }

    public int Id { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }

    // Navigation Properties
    public virtual ICollection<EInstalacao> EInstalacoes { get; set; }
  }
}
