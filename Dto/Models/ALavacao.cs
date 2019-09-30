namespace Dto.Models {
  public class ALavacao {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public int? Maquinas { get; set; }
    public decimal Aguam3 { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
