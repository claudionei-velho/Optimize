namespace Dto.Models {
  public class VetorH {
    public int Id { get; set; }
    public int Item { get; set; }
    public int VetorId { get; set; }
    public int HorarioId { get; set; }

    // Navigation Properties
    public virtual Vetor Vetor { get; set; }
    public virtual Horario Horario { get; set; }
  }
}
