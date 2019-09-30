using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class EUsuarioViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "UsuarioId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UsuarioIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int UsuarioId { get; set; }

    [Display(Name = "Ativo", ResourceType = typeof(Properties.Resources))]
    public bool Ativo { get; set; }

    [ScaffoldColumn(false)]
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Usuario Usuario { get; set; }
  }
}
