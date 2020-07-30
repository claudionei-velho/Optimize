using System;

namespace Dto.Models {
  public class Adjacencia {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int PontoId { get; set; }
    public int AdjacenteId { get; set; }
    public decimal? Distancia { get; set; }
    public int? Ciclo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Ponto Ponto { get; set; }
    public virtual Ponto Adjacente { get; set; }
  }
}
