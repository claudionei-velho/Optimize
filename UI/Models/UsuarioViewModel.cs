using System;
using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class UsuarioViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "UsuarioNome", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UsuarioNomeError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Nome { get; set; }

    [Display(Name = "UsuarioLogin", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LoginError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(256)]
    [EmailAddress]
    public string Login { get; set; }

    [Display(Name = "Senha", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SenhaError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(256, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [Display(Name = "ConfirmaSenha", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(256, MinimumLength = 4)]
    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessageResourceName = "ConfirmaSenhaError", 
                      ErrorMessageResourceType = typeof(Properties.Resources))]
    public string ConfirmaSenha { get; set; }

    [Display(Name = "Ativo", ResourceType = typeof(Properties.Resources))]
    public bool Ativo { get; set; }

    [ScaffoldColumn(false)]
    public int Perfil { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }
  }
}
