namespace Dto.Models {
  public class MapaLinha {
    public int Id { get; set; }
    public int LinhaId { get; set; }
    public string Sentido { get; set; }
    public int? AtendimentoId { get; set; }
    public string Descricao { get; set; }
    public string Arquivo { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
  }
}
