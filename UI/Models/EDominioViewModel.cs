using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class EDominioViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "DominioId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DominioIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int DominioId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Dominio Dominio { get; set; }
    public virtual Empresa Empresa { get; set; }
  }
}
