namespace Dto.Models {
  public class VeiculoAtt {
    public int Id { get; set; }
    public int Classe { get; set; }
    public int Attributo { get; set; }
    public string Conteudo { get; set; }

    public virtual CVeiculo CVeiculo { get; set; }
    public virtual CVeiculoAtt CVeiculoAtt { get; set; }
  }
}
