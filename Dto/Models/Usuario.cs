using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Usuario {
    public Usuario() {
      this.EUsuarios = new HashSet<EUsuario>();
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public int Perfil { get; set; }
    public bool Ativo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual ICollection<EUsuario> EUsuarios { get; set; }
  }
}
