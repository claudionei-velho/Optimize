using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;
using UI.Extensions;

namespace UI.Models {
  public class PontoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Prefixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PrefixoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Prefixo { get; set; }

    [Display(Name = "Identificacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "IdentificacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Identificacao { get; set; }

    [Display(Name = "Endereco", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EnderecoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(128)]
    public string Endereco { get; set; }

    [Display(Name = "EnderecoRef", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string EnderecoRef { get; set; }

    [Display(Name = "Cep", ResourceType = typeof(Properties.Resources))]
    public int? Cep { get; set; }

    [Display(Name = "Bairro", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Bairro { get; set; }

    [Display(Name = "Municipio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "MunicipioError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Municipio { get; set; }

    [Display(Name = "UfId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UfIdError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string UfId { get; set; }

    [Display(Name = "Intercambio", ResourceType = typeof(Properties.Resources))]
    public bool Intercambio { get; set; }

    [Display(Name = "Latitude", ResourceType = typeof(Properties.Resources))]
    [Latitude(-90, 90), DisplayFormat(DataFormatString = "{0:##0.0000######}", ApplyFormatInEditMode = true)]
    public decimal? Latitude { get; set; }

    [Display(Name = "Longitude", ResourceType = typeof(Properties.Resources))]
    [Longitude(-180, 180), DisplayFormat(DataFormatString = "{0:##0.0000######}", ApplyFormatInEditMode = true)]
    public decimal? Longitude { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
