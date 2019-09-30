using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class CorredorViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Prefixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PrefixoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Prefixo { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Denominacao { get; set; }

    [Display(Name = "Caracteristicas", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(512)]
    public string Caracteristicas { get; set; }

    [Display(Name = "Municipio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "MunicipioError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Municipio { get; set; }

    [Display(Name = "UfId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UfIdError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string UfId { get; set; }

    [Display(Name = "Extensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Extensao { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
