using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Empresa {
    public Empresa() {
      this.CLinhas = new HashSet<CLinha>();
      this.Corredores = new HashSet<Corredor>();
      this.Custos = new HashSet<Custo>();
      this.CustosMod = new HashSet<CustoMod>();
      this.DemandasMod = new HashSet<DemandaMod>();
      this.ECVeiculos = new HashSet<ECVeiculo>();
      this.EDominios = new HashSet<EDominio>();
      this.EPeriodos = new HashSet<EPeriodo>();
      this.EUsuarios = new HashSet<EUsuario>();
      this.Instalacoes = new HashSet<Instalacao>();
      this.Linhas = new HashSet<Linha>();
      this.Operacoes = new HashSet<Operacao>();
      this.Pesquisas = new HashSet<Pesquisa>();
      this.Pontos = new HashSet<Ponto>();
      this.Tarifas = new HashSet<Tarifa>();
      this.TCategorias = new HashSet<TCategoria>();
      this.Terminais = new HashSet<Terminal>();
      this.Vetores = new HashSet<Vetor>();
      this.Veiculos = new HashSet<Veiculo>();
      this.TarifaMods = new HashSet<TarifaMod>();
      this.Troncos = new HashSet<Tronco>();
      this.SpecVeiculos = new HashSet<SpecVeiculo>();
      this.FrotaEtarias = new HashSet<FrotaEtaria>();
    }

    public int Id { get; set; }
    public string Razao { get; set; }
    public string Fantasia { get; set; }
    public string Cnpj { get; set; }
    public string IEstadual { get; set; }
    public string IMunicipal { get; set; }
    public string Endereco { get; set; }
    public string EnderecoNo { get; set; }
    public string Complemento { get; set; }
    public int? Cep { get; set; }
    public string Bairro { get; set; }
    public string Municipio { get; set; }
    public int? MunicipioId { get; set; }
    public string UfId { get; set; }
    public string PaisId { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Termino { get; set; }
    public string Logo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Municipio Cidade { get; set; }
    public virtual Pais Pais { get; set; }

    public virtual ICollection<CLinha> CLinhas { get; set; }
    public virtual ICollection<Corredor> Corredores { get; set; }
    public virtual ICollection<Custo> Custos { get; set; }
    public virtual ICollection<CustoMod> CustosMod { get; set; }
    public virtual ICollection<DemandaMod> DemandasMod { get; set; }
    public virtual ICollection<ECVeiculo> ECVeiculos { get; set; }
    public virtual ICollection<EDominio> EDominios { get; set; }
    public virtual ICollection<EPeriodo> EPeriodos { get; set; }
    public virtual ICollection<EUsuario> EUsuarios { get; set; }
    public virtual ICollection<Instalacao> Instalacoes { get; set; }
    public virtual ICollection<Linha> Linhas { get; set; }
    public virtual ICollection<Operacao> Operacoes { get; set; }
    public virtual ICollection<Pesquisa> Pesquisas { get; set; }
    public virtual ICollection<Ponto> Pontos { get; set; }
    public virtual ICollection<Tarifa> Tarifas { get; set; }
    public virtual ICollection<TCategoria> TCategorias { get; set; }
    public virtual ICollection<Terminal> Terminais { get; set; }
    public virtual ICollection<Vetor> Vetores { get; set; }
    public virtual ICollection<Veiculo> Veiculos { get; set; }
    public virtual ICollection<TarifaMod> TarifaMods { get; set; }
    public virtual ICollection<Tronco> Troncos { get; set; }
    public virtual ICollection<SpecVeiculo> SpecVeiculos { get; set; }
    public virtual ICollection<FrotaEtaria> FrotaEtarias { get; set; }
  }
}
