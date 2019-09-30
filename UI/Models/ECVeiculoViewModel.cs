using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class ECVeiculoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "ClasseId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClasseIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int ClasseId { get; set; }

    [Display(Name = "Minimo", ResourceType = typeof(Properties.Resources))]
    public int? Minimo { get; set; }

    [Display(Name = "Maximo", ResourceType = typeof(Properties.Resources))]
    public int? Maximo { get; set; }

    [Display(Name = "Passageirom2", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "Passageirom2Error", ErrorMessageResourceType = typeof(Properties.Resources))]
    public byte Passageirom2 { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual CVeiculo CVeiculo { get; set; }
  }
}
