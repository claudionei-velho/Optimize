using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class EInstalacao {
    public EInstalacao() {
      this.Garagens = new HashSet<AGaragem>();
      this.Administracoes = new HashSet<AAdmin>();
      this.Abastecimentos = new HashSet<AAbastece>();
      this.Lavacoes = new HashSet<ALavacao>();
      this.Inspecoes = new HashSet<AInspecao>();
      this.Lubrificacoes = new HashSet<ALubrifica>();
      this.Manutencoes = new HashSet<AMantem>();
      this.Funilarias = new HashSet<AFunilaria>();
      this.Almoxarifados = new HashSet<AAlmox>();
      this.Trafegos = new HashSet<ATrafego>();
      this.Estacionamentos = new HashSet<AEstaciona>();
    }

    public int Id { get; set; }
    public int InstalacaoId { get; set; }
    public int PropositoId { get; set; }
    public decimal? AreaCoberta { get; set; }
    public decimal? AreaTotal { get; set; }
    public int? QtdEmpregados { get; set; }
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Termino { get; set; }
    public bool Efluentes { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Instalacao Instalacao { get; set; }
    public virtual FInstalacao FInstalacao { get; set; }

    public virtual ICollection<AGaragem> Garagens { get; set; }
    public virtual ICollection<AAdmin> Administracoes { get; set; }
    public virtual ICollection<AAbastece> Abastecimentos { get; set; }
    public virtual ICollection<ALavacao> Lavacoes { get; set; }
    public virtual ICollection<AInspecao> Inspecoes { get; set; }
    public virtual ICollection<ALubrifica> Lubrificacoes { get; set; }
    public virtual ICollection<AMantem> Manutencoes { get; set; }
    public virtual ICollection<AFunilaria> Funilarias { get; set; }
    public virtual ICollection<AAlmox> Almoxarifados { get; set; }
    public virtual ICollection<ATrafego> Trafegos { get; set; }
    public virtual ICollection<AEstaciona> Estacionamentos { get; set; }
  }
}
