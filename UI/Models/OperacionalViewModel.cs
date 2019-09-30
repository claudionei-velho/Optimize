using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Dto.Models;

namespace UI.Models {
  public class OperacionalViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Key, Column(Order = 2)]
    [Display(Name = "Prefixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PrefixoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Prefixo { get; set; }

    [Key, Column(Order = 3), StringLength(16)]
    public string Tipo { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(128)]
    public string Denominacao { get; set; }

    [Key, Column(Order = 4)]
    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public string DiaOperacao { get; set; }

    [Display(Name = "Funcao", ResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Funcao { get; set; }

    [Display(Name = "Extensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Extensao { get; set; }

    [Display(Name = "ViagensDU", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int? ViagensDU { get; set; }

    [Display(Name = "PercursoDU", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? PercursoDU { get; set; }

    [Display(Name = "ViagensSab", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int? ViagensSab { get; set; }

    [Display(Name = "PercursoSab", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? PercursoSab { get; set; }

    [Display(Name = "ViagensDom", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int? ViagensDom { get; set; }

    [Display(Name = "PercursoDom", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? PercursoDom { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
