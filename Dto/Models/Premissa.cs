using System;

namespace Dto.Models {
  public class Premissa {
    public int EmpresaId { get; set; }
    public decimal JornadaDia { get; set; }
    public decimal JornadaMax { get; set; }
    public decimal JornadaSemana { get; set; }
    public decimal InterJornada { get; set; }
    public decimal IntraJornadaMin { get; set; }
    public decimal IntraJornadaMax { get; set; }
    public int? DeslocaInicial { get; set; }
    public int? Deslocamento { get; set; }
    public int VetorPadrao { get; set; }
    public TimeSpan NoturnoInicio { get; set; }
    public TimeSpan NoturnoTermino { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
