using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dto.Models;

namespace Dal {
  public class DataContext : DbContext {
    static DataContext() {
      Database.SetInitializer<DataContext>(null);
    }

    public DataContext() : base("Name=DataContext") { }

    public DbSet<Adjacencia> Adjacencias { get; set; }
    public DbSet<Arco> Arcos { get; set; }
    public DbSet<ArcoV> ArcosV { get; set; }
    public DbSet<Bacia> Bacias { get; set; }
    public DbSet<ClassLinha> ClassLinhas { get; set; }
    public DbSet<Dominio> Dominios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<EUsuario> EUsuarios { get; set; }
    public DbSet<FInstalacao> FInstalacoes { get; set; }
    public DbSet<ISinotico> ISinoticos { get; set; }
    public DbSet<OperLinha> OperLinhas { get; set; }
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Periodo> Periodos { get; set; }
    public DbSet<Uf> Ufs { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Atendimento> Atendimentos { get; set; }
    public DbSet<Carroceria> Carrocerias { get; set; }
    public DbSet<Chassi> Chassis { get; set; }
    public DbSet<CLinha> CLinhas { get; set; }
    public DbSet<Corredor> Corredores { get; set; }
    public DbSet<CustoCo> CustosCo { get; set; }
    public DbSet<CustoLn> CustosLn { get; set; }
    public DbSet<CVeiculo> CVeiculos { get; set; }
    public DbSet<CVeiculoAtt> CVeiculosAtts { get; set; }
    public DbSet<ECVeiculo> ECVeiculos { get; set; }
    public DbSet<EDominio> EDominios { get; set; }
    public DbSet<EPeriodo> EPeriodos { get; set; }
    public DbSet<FViagem> FViagens { get; set; }
    public DbSet<Horario> Horarios { get; set; }
    public DbSet<ItAtendimento> ItAtendimentos { get; set; }
    public DbSet<Itinerario> Itinerarios { get; set; }
    public DbSet<ItinerarioDistinct> ItinerariosDistinct { get; set; }
    public DbSet<Linha> Linhas { get; set; }
    public DbSet<LnCorredor> LnCorredores { get; set; }
    public DbSet<LnPesquisa> LnPesquisas { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<MatrizH> MatrizesH { get; set; }
    public DbSet<Motor> Motores { get; set; }
    public DbSet<Municipio> Municipios { get; set; }
    public DbSet<Ocupacao> Ocupacoes { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Pesquisa> Pesquisas { get; set; }
    public DbSet<Ponto> Pontos { get; set; }
    public DbSet<Premissa> Premissas { get; set; }
    public DbSet<PrLinha> PrLinhas { get; set; }    
    public DbSet<PtAtendimento> PtAtendimentos { get; set; }
    public DbSet<PtLinha> PtLinhas { get; set; }
    public DbSet<ReceitaCo> ReceitasCo { get; set; }
    public DbSet<ReceitaLn> ReceitasLn { get; set; }
    public DbSet<Referencia> Referencias { get; set; }
    public DbSet<Renovacao> Renovacoes { get; set; }
    public DbSet<Rubrica> Rubricas { get; set; }
    public DbSet<Tarifa> Tarifas { get; set; }
    public DbSet<TarifaMod> TarifaMods { get; set; }
    public DbSet<TCategoria> TCategorias { get; set; }
    public DbSet<Terminal> Terminais { get; set; }
    public DbSet<TServico> TServicos { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<VeiculoAtt> VeiculosAtt { get; set; }
    public DbSet<Vetor> Vetores { get; set; }
    public DbSet<VetorH> VetoresH { get; set; }
    public DbSet<Viagem> Viagens { get; set; }
    public DbSet<FAjuste> FAjustes { get; set; }
    public DbSet<TotalViagem> TotalViagens { get; set; }
    public DbSet<Dimensionamento> Dimensionamentos { get; set; }
    public DbSet<PtOrigem> PtOrigens { get; set; }
    public DbSet<PtDestino> PtDestinos { get; set; }
    public DbSet<MapaLinha> MapasLinha { get; set; }
    public DbSet<Sinotico> Sinoticos { get; set; }
    public DbSet<Custo> Custos { get; set; }
    public DbSet<CustoMod> CustoMods { get; set; }
    public DbSet<LnTerminal> LnTerminais { get; set; }
    public DbSet<Tronco> Troncos { get; set; }
    public DbSet<ItTronco> ItTroncos { get; set; }
    public DbSet<PtTronco> PtTroncos { get; set; }
    public DbSet<LnTronco> LnTroncos { get; set; }
    public DbSet<SpecVeiculo> SpecVeiculos { get; set; }
    public DbSet<Instalacao> Instalacoes { get; set; }
    public DbSet<EInstalacao> EInstalacoes { get; set; }
    public DbSet<AGaragem> Garagens { get; set; }
    public DbSet<AAdmin> Administracoes { get; set; }
    public DbSet<AAbastece> Abastecimentos { get; set; }
    public DbSet<ALavacao> Lavacoes { get; set; }
    public DbSet<AInspecao> Inspecoes { get; set; }
    public DbSet<ALubrifica> Lubrificacoes { get; set; }
    public DbSet<AMantem> Manutencoes { get; set; }
    public DbSet<AFunilaria> Funilarias { get; set; }
    public DbSet<AAlmox> Almoxarifados { get; set; }
    public DbSet<ATrafego> Trafegos { get; set; }
    public DbSet<AEstaciona> Estacionamentos { get; set; }
    public DbSet<FxEtaria> FxEtarias { get; set; }
    public DbSet<FrotaEtaria> FrotaEtarias { get; set; }
    public DbSet<DiaTrabalho> DiasTrabalho { get; set; }

    // Reports (Database Views)
    public DbSet<FuUtil> FuUteis { get; set; }
    public DbSet<Operacional> Operacionais { get; set; }
    public DbSet<PlanOperacional> PlanOperacionais { get; set; }
    public DbSet<PeriodoTipico> PeriodosTipicos { get; set; }
    public DbSet<ViagemLinha> ViagensLinha { get; set; }
    public DbSet<ViagemHora> ViagensHora { get; set; }
    public DbSet<DemandaMes> DemandasMes { get; set; }
    public DbSet<DemandaMod> DemandasMod { get; set; }
    public DbSet<DemandaAno> DemandasAno { get; set; }
    public DbSet<Tecnical> Tecnicals { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

      // Ignore Properties
      modelBuilder.Entity<ReceitaCo>().Ignore(p => p.Impostos);
      modelBuilder.Entity<ReceitaCo>().Ignore(p => p.Liquida);

      modelBuilder.Entity<ReceitaLn>().Ignore(p => p.Impostos);
      modelBuilder.Entity<ReceitaLn>().Ignore(p => p.Liquida);

      base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges() {
      foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(entry =>
                                          entry.Entity.GetType().GetProperty("Cadastro") != null)) {
        if (entry.State == EntityState.Added) {
          entry.Property("Cadastro").CurrentValue = DateTime.Now;
        }
        if (entry.State == EntityState.Modified) {
          entry.Property("Cadastro").IsModified = false;
        }
      }
      return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
      foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(entry =>
                                          entry.Entity.GetType().GetProperty("Cadastro") != null)) {
        if (entry.State == EntityState.Added) {
          entry.Property("Cadastro").CurrentValue = DateTime.Now;
        }
        if (entry.State == EntityState.Modified) {
          entry.Property("Cadastro").IsModified = false;
        }
      }
      return await base.SaveChangesAsync(cancellationToken);
    }
  }
}
