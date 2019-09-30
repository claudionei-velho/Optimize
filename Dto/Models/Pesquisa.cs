using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Pesquisa {
    public Pesquisa() {
      this.LnPesquisas = new HashSet<LnPesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Identificacao { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime Termino { get; set; }
    public int? TerminalId { get; set; }
    public int? TroncoId { get; set; }
    public int? CorredorId { get; set; }
    public int? OperacaoId { get; set; }
    public int? Classificacao { get; set; }    
    public bool Interna { get; set; }
    public string Fornecedor { get; set; }
    public string Contrato { get; set; }
    public bool Uteis { get; set; }
    public bool Sabados { get; set; }
    public bool Domingos { get; set; }
    public string Responsavel { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Empresa Empresa { get; set; }
    public virtual Terminal Terminal { get; set; }
    public virtual Tronco Tronco { get; set; }
    public virtual Corredor Corredor { get; set; }
    public virtual Operacao Operacao { get; set; }
    public virtual CLinha CLinha { get; set; }
    
    public virtual ICollection<LnPesquisa> LnPesquisas { get; set; }
  }
}
