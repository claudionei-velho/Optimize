namespace Dto.Models {
  public class ALubrifica {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public byte? Lavacao { get; set; }
    public bool Ceramico { get; set; }
    public bool Protecao { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
