namespace Dto.Models {
  public class ArcoV {
    public int Id { get; set; }    
    public int ArcoId { get; set; }
    public int Item { get; set; }
    public int VetorId { get; set; }

    // Navigation Properties
    public virtual Arco Arco { get; set; }
    public virtual Vetor Vetor { get; set; }
  }
}
