namespace Dto.Models {
  public class ATrafego {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public byte? Plantao { get; set; }
    public byte? Reserva { get; set; }
    public byte? Equipamentos { get; set; }
    public byte? Mobiliario { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
