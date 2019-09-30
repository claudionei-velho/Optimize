namespace Dto.Models {
  public class AAlmox {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public decimal? Area { get; set; }
    public bool Especifico { get; set; }
    public bool Estoque { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
