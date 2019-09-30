namespace Dto.Models {
  public class AInspecao {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public int Rampas { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
