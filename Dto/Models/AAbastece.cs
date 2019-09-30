namespace Dto.Models {
  public class AAbastece {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public int PavimentoId { get; set; }
    public int Bombas { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
    public virtual Via Via { get; set; }
  }
}
