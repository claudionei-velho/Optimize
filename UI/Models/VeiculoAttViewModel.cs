using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class VeiculoAttViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Classe", ResourceType = typeof(Properties.Resources))]
    public int Classe { get; set; }

    [Display(Name = "Atributo", ResourceType = typeof(Properties.Resources))]
    public int Attributo { get; set; }

    [Display(Name = "Unidade", ResourceType = typeof(Properties.Resources))]
    public string Unidade {
      get {
        return this.CVeiculoAtt.Unidade;
      }
    }

    [Display(Name = "Conteudo", ResourceType = typeof(Properties.Resources))]
    public string Conteudo { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual CVeiculoAtt CVeiculoAtt { get; set; }
  }
}
