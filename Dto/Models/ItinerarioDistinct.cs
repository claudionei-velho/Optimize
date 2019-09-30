namespace Dto.Models {
  public class ItinerarioDistinct {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public string Prefixo { get; set; }
    public string Sentido { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
  }
}
