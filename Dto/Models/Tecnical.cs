using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dto.Models {
  public class Tecnical {
    private readonly char[] charsToTrim = new char[] { ' ', ';', '/' };

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public string Viagem { get; set; }
    public bool Uteis { get; set; }
    public bool Sabados { get; set; }
    public bool Domingos { get; set; }
    public bool Escolar { get; set; }

    [NotMapped]
    public string DiasOp {
      get {
        StringBuilder aux = new StringBuilder();
        if (this.Uteis) {
          aux.Append("Dias Úteis; ");
        }
        if (this.Sabados) {
          aux.Append("Sábados; ");
        }
        if (this.Domingos) {
          aux.Append("Domingos");
        }
        return aux.ToString().Trim(charsToTrim);
      }
    }

    public int DominioId { get; set; }
    public int OperacaoId { get; set; }
    public int Classificacao { get; set; }
    public bool Captacao { get; set; }
    public bool Transporte { get; set; }
    public bool Distribuicao { get; set; }

    [NotMapped]
    public string Funcoes {
      get {
        StringBuilder aux = new StringBuilder();
        if (this.Captacao) {
          aux.Append("Captação; ");
        }
        if (this.Transporte) {
          aux.Append("Transporte; ");
        }
        if (this.Distribuicao) {
          aux.Append("Distribuição");
        }
        return aux.ToString().Trim(charsToTrim);
      }
    }

    public decimal? ExtensaoAB { get; set; }
    public decimal? ExtensaoBA { get; set; }

    [NotMapped]
    public decimal? Extensao {
      get {
        return this.ExtensaoAB + this.ExtensaoBA;
      }
    }

    public int? LoteId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual EDominio EDominio { get; set; }
    public virtual Operacao Operacao { get; set; }
    public virtual CLinha CLinha { get; set; }
    public virtual Lote Lote { get; set; }
  }
}
