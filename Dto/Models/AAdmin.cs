namespace Dto.Models {
  public class AAdmin {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public decimal? Requisitom2 { get; set; }
    public decimal Disponivelm2 { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}

