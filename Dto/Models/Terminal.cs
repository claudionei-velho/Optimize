using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Terminal {
    public Terminal() {
      this.TServicos = new HashSet<TServico>();
      this.LnTerminais = new HashSet<LnTerminal>();
      this.Pesquisas = new HashSet<Pesquisa>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public string Cnpj { get; set; }
    public string IEstadual { get; set; }
    public string IMunicipal { get; set; }
    public string Endereco { get; set; }
    public string EnderecoNo { get; set; }
    public string Complemento { get; set; }
    public int? Cep { get; set; }
    public string Bairro { get; set; }
    public string Municipio { get; set; }
    public string UfId { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Termino { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<TServico> TServicos { get; set; }
    public virtual ICollection<LnTerminal> LnTerminais { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
  }
}
