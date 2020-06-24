using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class EmpresaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Razao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "RazaoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Razao { get; set; }

    [Display(Name = "Fantasia", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "FantasiaError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Fantasia { get; set; }

    [Display(Name = "Cnpj", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "CnpjError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [RegularExpression(@"(^(\d{2}.\d{3}.\d{3}/\d{4}-\d{2})|(\d{14})$)"), StringLength(32)]
    public string Cnpj { get; set; }

    [Display(Name = "IEstadual", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string IEstadual { get; set; }

    [Display(Name = "IMunicipal", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string IMunicipal { get; set; }

    [Display(Name = "Endereco", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EnderecoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(128)]
    public string Endereco { get; set; }

    [Display(Name = "EnderecoNo", ResourceType = typeof(Properties.Resources))]
    [StringLength(8)]
    public string EnderecoNo { get; set; }

    [Display(Name = "Complemento", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Complemento { get; set; }

    [Display(Name = "Cep", ResourceType = typeof(Properties.Resources))]
    public int? Cep { get; set; }

    [Display(Name = "Bairro", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Bairro { get; set; }

    [Display(Name = "Municipio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "MunicipioError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Municipio { get; set; }
    public int? MunicipioId { get; set; }

    [Display(Name = "UfId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UfIdError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string UfId { get; set; }

    [Display(Name = "PaisId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PaisIdError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(8)]
    public string PaisId { get; set; }

    [Display(Name = "Telefone", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Telefone { get; set; }

    [Display(Name = "Email", ResourceType = typeof(Properties.Resources))]
    [EmailAddress, StringLength(256)]
    public string Email { get; set; }

    [Display(Name = "Inicio", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Inicio { get; set; }

    [Display(Name = "Termino", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Termino { get; set; }

    [ScaffoldColumn(false)]
    public string Logo { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Municipio Cidade { get; set; }
    public virtual Pais Pais { get; set; }
  }
}
