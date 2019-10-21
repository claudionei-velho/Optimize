﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class SinoticoViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return new Workday().Data[DiaId];
      }
    }

    [Key, Column(Order = 2)]
    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

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
    public virtual Linha Linha { get; set; }
    public virtual ISinotico ISinotico { get; set; }
  }
}