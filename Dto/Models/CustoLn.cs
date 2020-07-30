namespace Dto.Models {
  public class CustoLn {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int RubricaId { get; set; }
    public int SubtotalId { get; set; }
    public int TotalId { get; set; }
    public decimal? Percurso { get; set; }
    public decimal? Coeficiente { get; set; }
    public decimal? Custo { get; set; }

    // Navigation Properties
    public virtual Rubrica Rubrica { get; set; }
  }
}
