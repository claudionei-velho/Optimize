using System;
using System.ComponentModel.DataAnnotations;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class VeiculoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Numero", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "NumeroError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Numero { get; set; }

    [Display(Name = "Cor", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "CorError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Cor { get; set; }

    [Display(Name = "Classe", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClasseError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Classe { get; set; }

    [Display(Name = "Categoria", ResourceType = typeof(Properties.Resources))]
    public int? Categoria { get; set; }

    public string CategoriaCap {
      get {
        return Bll.Lists.Categoria.Data[this.Categoria ?? 0];
      }
    }

    [Display(Name = "Placa", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PlacaError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Placa { get; set; }

    [Display(Name = "Renavam", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "RenavamError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Renavam { get; set; }

    [Display(Name = "Antt", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string Antt { get; set; }

    [Display(Name = "InicioOperacao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Inicio { get; set; }

    [Display(Name = "Inativo", ResourceType = typeof(Properties.Resources))]
    public bool Inativo { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual Empresa Empresa { get; set; }
    public virtual Chassi Chassi { get; set; }
    public virtual Carroceria Carroceria { get; set; }
  }
}
