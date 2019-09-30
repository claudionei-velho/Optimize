namespace Dto.Models {
  public class AMantem {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public decimal? Area { get; set; }
    public int? PontosAr { get; set; }
    public byte Eletricidade { get; set; }
    public int? Elevadores { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
