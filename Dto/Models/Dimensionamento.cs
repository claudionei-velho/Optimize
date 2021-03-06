﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class Dimensionamento {
    public int PesquisaId { get; set; }
    public int LinhaId { get; set; }
    public int DiaId { get; set; }
    public int PeriodoId { get; set; }
    public string Sentido { get; set; }
    public int QtdViagens { get; set; }
    public TimeSpan Inicio { get; set; }
    public TimeSpan Termino { get; set; }

    [NotMapped]
    public int Duracao {
      get {
        return ((int)this.Termino.Subtract(this.Inicio).TotalMinutes < 0) ?
                   1440 + (int)this.Termino.Subtract(this.Inicio).TotalMinutes : 
                   (int)this.Termino.Subtract(this.Inicio).TotalMinutes;
      }
    }

    public int? Ociosidade { get; set; }
    public int Passageiros { get; set; }
    public int Ajustado { get; set; }
    public int Critica { get; set; }
    public int CriticaAjuste { get; set; }

    [NotMapped]
    public int? Media {
      get {
        try {
          return (int)Math.Ceiling((decimal)Passageiros / QtdViagens);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public int? MediaAjuste {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / QtdViagens);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    public int Desvio { get; set; }
    public int DesvioAjuste { get; set; }

    [NotMapped]
    public int? Intervalo {
      get {
        try {
          return Duracao / QtdViagens;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public decimal? Fluxo {
      get {
        try {
          return (decimal)Passageiros / Duracao;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public decimal? FluxoAjuste {
      get {
        try {
          return (decimal)Ajustado / Duracao;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public int LotacaoE {
      get {
        return (MediaAjuste ?? 0) + DesvioAjuste;
      }
    }

    [NotMapped]
    public int? PrognosticoE {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / LotacaoE);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public int? IntervaloE {
      get {
        try {
          return Duracao / PrognosticoE;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    public int LotacaoP { get; set; }

    [NotMapped]
    public int? PrognosticoP {
      get {
        try {
          return (int)Math.Ceiling((decimal)Ajustado / LotacaoP);
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    [NotMapped]
    public int? IntervaloP {
      get {
        try {
          return Duracao / PrognosticoP;
        }
        catch (DivideByZeroException) {
          return null;
        }
      }
    }

    public int? CicloAB { get; set; }
    public int? CicloBA { get; set; }


    [NotMapped]
    public int? Veiculos {
      get {
        int? result; 
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (Intervalo ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > this.QtdViagens) {
          result = this.QtdViagens;
        }
        return result;
      }
    }

    [NotMapped]
    public int? VeiculosE {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (IntervaloE ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > (PrognosticoE ?? 0)) {
          result = PrognosticoE ?? 0;
        }
        return result;
      }
    }

    [NotMapped]
    public int? VeiculosP {
      get {
        int? result;
        try {
          result = (int)Math.Ceiling((decimal)Tempo / (IntervaloP ?? 0));
        }
        catch (DivideByZeroException) {
          result = null;
        }
        if ((result ?? 0) > (PrognosticoP ?? 0)) {
          result = PrognosticoP ?? 0;
        }
        return result;
      }
    }

    public decimal? Extensao { get; set; }

    [NotMapped]
    public decimal? KmTotal {
      get {
        return QtdViagens * Extensao;
      }
    }

    [NotMapped]
    public decimal? KmTotalE {
      get {
        return PrognosticoE * Extensao;
      }
    }

    [NotMapped]
    public decimal? KmTotalP {
      get {
        return PrognosticoP * Extensao;
      }
    }

    private int Tempo {
      get {
        return (this.CicloAB ?? 0) >= (this.CicloBA ?? 0) ? (this.CicloAB ?? 0) : (this.CicloBA ?? 0);
      }
    }

    // Navigation Properties
    public virtual Pesquisa Pesquisa { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }
  }
}
