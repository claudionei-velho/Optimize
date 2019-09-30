namespace Dto.Models {
  public class Operacional {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public string Sentido { get; set; }
    public string DiaOperacao { get; set; }
    public string Funcao { get; set; }
    public decimal? Extensao { get; set; }
    public int? ViagensDU { get; set; }
    public decimal? PercursoDU { get; set; }
    public int? ViagensSab { get; set; }
    public decimal? PercursoSab { get; set; }
    public int? ViagensDom { get; set; }
    public decimal? PercursoDom { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
  }
}
