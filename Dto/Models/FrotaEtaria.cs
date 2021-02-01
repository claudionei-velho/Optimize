namespace Dto.Models {
  public class FrotaEtaria {
    public int EmpresaId { get; set; }
    public int EtariaId { get; set; }
    public int? Micro { get; set; }
    public int? Mini { get; set; }
    public int? Midi { get; set; }
    public int? Basico { get; set; }
    public int? Padron { get; set; }
    public int? Especial { get; set; }
    public int? Articulado { get; set; }
    public int? BiArticulado { get; set; }    
    public int Frota { get; set; }
    public decimal? Ratio { get; set; }
    public decimal? EqvIdade { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual FxEtaria FxEtaria { get; set; }
  }
}
