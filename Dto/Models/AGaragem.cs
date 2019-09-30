namespace Dto.Models {
  public class AGaragem {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public int Frota { get; set; }
    public decimal Requisitom2 { get; set; }
    public decimal? Minimom2 { get; set; }
    public decimal Disponivelm2 { get; set; }

    // Navigation Properites
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
