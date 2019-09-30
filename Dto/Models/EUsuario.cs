using System;

namespace Dto.Models {
  public class EUsuario {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int UsuarioId { get; set; }
    public bool Ativo { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Usuario Usuario { get; set; }
  }
}
