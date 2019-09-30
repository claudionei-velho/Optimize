namespace Dto.Models {
  public class AFunilaria {
    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public decimal? Area { get; set; }
    public bool Isolada { get; set; }
    public byte? PPoluicao { get; set; }
    public byte? Exaustao { get; set; }

    // Navigation Properties
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
