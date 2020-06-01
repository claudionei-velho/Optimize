using System;
using System.ComponentModel.DataAnnotations;

using Bll.Services;
using Dto.Extensions;
using Dto.Models;

namespace UI.Models {
  public class InstalacaoViewModel {
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
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Municipio { get; set; }

    [Display(Name = "UfId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "UfIdError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string UfId { get; set; }

    [Display(Name = "Telefone", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Telefone { get; set; }

    [Display(Name = "Email", ResourceType = typeof(Properties.Resources))]
    [EmailAddress, StringLength(256)]
    public string Email { get; set; }

    [Display(Name = "AreaCoberta", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}")]
    public decimal? AreaCoberta {
      get {
        using EInstalacaoService instalacao = new EInstalacaoService();
        return instalacao.TotalAreaCoberta(q => q.InstalacaoId == this.Id);
      }
    }

    [Display(Name = "AreaTotal", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}")]
    public decimal? AreaTotal {
      get {
        using EInstalacaoService instalacao = new EInstalacaoService();
        return instalacao.TotalArea(q => q.InstalacaoId == this.Id);
      }
    }

    [Display(Name = "QtdEmpregados", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public decimal? QtdEmpregados {
      get {
        using EInstalacaoService instalacao = new EInstalacaoService();
        return instalacao.TotalEmpregados(q => q.InstalacaoId == this.Id);
      }
    }

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
