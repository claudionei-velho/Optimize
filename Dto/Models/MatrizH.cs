using System;

namespace Dto.Models {
  public class MatrizH {
    public int Id { get; set; }
    public int EmpresaId { get; set; }    
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public int DiaId { get; set; }
    public int? Item { get; set; }
    public TimeSpan Inicio { get; set; }
    public TimeSpan Termino { get; set; }
    public int? Duracao { get; set; }
    public string Sentido { get; set; }
    public int? PInicioId { get; set; }
    public int? PTerminoId { get; set; }

    // Navigation Properties
    public virtual Ponto PInicio { get; set; }
    public virtual Ponto PTermino { get; set; }
  }
}
