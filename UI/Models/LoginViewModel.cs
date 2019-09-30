using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class LoginViewModel {
    [Display(Name = "UsuarioLogin", ResourceType = typeof(Properties.Resources))]
    [EmailAddress, StringLength(256)]
    public string Login { get; set; }

    [Display(Name = "Senha", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SenhaError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(256, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Senha { get; set; }
  }
}
