using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class SinoticoViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "PesquisaId", ResourceType = typeof(Properties.Resources))]
    public int PesquisaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Key, Column(Order = 2)]
    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return Workday.Items[DiaId];
      }
    }

    [Key, Column(Order = 3)]
    [Display(Name = "SinoticoId", ResourceType = typeof(Properties.Resources))]
    public int SinoticoId { get; set; }

    [Display(Name = "IndiceAtual", ResourceType = typeof(Properties.Resources))]
    public string IndiceAtual { get; set; }

    [Display(Name = "DimensionaE", ResourceType = typeof(Properties.Resources))]
    public string DimensionaE { get; set; }

    [Display(Name = "DimensionaP", ResourceType = typeof(Properties.Resources))]
    public string DimensionaP { get; set; }

    [Display(Name = "EvolucaoE", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:P2}"), Editable(false)]
    public decimal? EvolucaoE { get; set; }

    [Display(Name = "EvolucaoP", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:P2}"), Editable(false)]
    public decimal? EvolucaoP { get; set; }

    // Navigation Properties
    public virtual Pesquisa Pesquisa { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual ISinotico ISinotico { get; set; }
  }
}
