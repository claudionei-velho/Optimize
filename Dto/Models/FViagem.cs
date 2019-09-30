namespace Dto.Models {
  public class FViagem {
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int PontoId { get; set; }
    public int? Embarques { get; set; }
    public int? Desembarques { get; set; }
    public int? Acumulado { get; set; }

    public virtual PtLinha PtLinha { get; set; }
    public virtual Viagem Viagem { get; set; }
  }
}
