using System;

namespace Dto.Models {
  public class Oferta {
    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int Categoria { get; set; }
    public int Passageiros { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Linha Linha { get; set; }
    public virtual TCategoria TCategoria { get; set; }
  }
}
