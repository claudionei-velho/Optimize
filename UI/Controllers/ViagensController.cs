using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class ViagensController : Controller {
    private ViagemService viagens = new ViagemService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ViagemViewModel, Viagem>().ReverseMap();
                                          }).CreateMapper();

    // GET: Viagens
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.viagens = new ViagemService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ViagemViewModel>>(await viagens.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Viagens
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<ViagemViewModel>>(
                          await viagens.GetAllAsync(q => q.LnPesquisa.PesquisaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: Viagens/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Viagem viagem = await viagens.GetFirstAsync(v => v.Id == id);
      if (viagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ViagemViewModel>(viagem);
      return View(viewModel);
    }

    // GET: Viagens/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LnPesquisaService linhas = new LnPesquisaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Linha.Prefixo + " | " + q.Linha.Denominacao
            }), "Id", "Name");
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name");
      using (HorarioService horarios = new HorarioService(user.ID)) {
        ViewBag.HorarioId = new SelectList(horarios.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Inicio }, 
            orderBy: q => q.OrderBy(h => h.Inicio)), "Id", "Name");
      }
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Ponto.Prefixo }), "Id", "Name");
      }
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(veiculos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name");
      }
      return View();
    }

    // POST: Viagens/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ViagemViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LnPesquisaService linhas = new LnPesquisaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Linha.Prefixo + " | " + q.Linha.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (HorarioService horarios = new HorarioService(user.ID)) {
        ViewBag.HorarioId = new SelectList(await horarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Inicio },
            orderBy: q => q.OrderBy(h => h.Inicio)), "Id", "Name", viewModel.HorarioId);
      }
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Ponto.Prefixo }), "Id", "Name", viewModel.PontoId);
      }
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(await veiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name", viewModel.VeiculoId);
      }

      try {
        if (ModelState.IsValid) {
          Viagem viagem = mapper.Map<Viagem>(viewModel);
          await viagens.Insert(viagem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Viagens/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Viagem viagem = await viagens.GetByIdAsync(id);
      if (viagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ViagemViewModel>(viagem);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      int linhaId = 0;
      int diaId = Workday.GetWorkday(viewModel.Data);
      using (Services<LnPesquisa> lnPesquisas = new Services<LnPesquisa>()) {
        linhaId = lnPesquisas.GetById(viewModel.LinhaId).LinhaId;
        using LnPesquisaService linhas = new LnPesquisaService(user.ID);
        ViewBag.LinhaId = new SelectList(
                                  await linhas.GetSelectAsync(
                                            q => new { Id = q.Id.ToString(), 
                                                       Name = q.Linha.Prefixo + " | " + q.Linha.Denominacao }
                                        ), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (HorarioService horarios = new HorarioService(user.ID)) {
        ViewBag.HorarioId = new SelectList(await horarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Inicio },
            q => (q.LinhaId == linhaId) && (q.DiaId == diaId) &&
                         q.Sentido.Equals(viewModel.Sentido),
            q => q.OrderBy(h => h.Inicio)), "Id", "Name", viewModel.HorarioId);
      }
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Ponto.Prefixo }), "Id", "Name", viewModel.PontoId);
      }
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(await veiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name", viewModel.VeiculoId);
      }
      return View(viewModel);
    }

    // POST: Viagens/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ViagemViewModel viewModel) {      
      int diaId = Workday.GetWorkday(viewModel.Data);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LnPesquisaService linhas = new LnPesquisaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Linha.Prefixo + " | " + q.Linha.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (HorarioService horarios = new HorarioService(user.ID)) {
        int linhaId;
        using (Services<LnPesquisa> lPesquisas = new Services<LnPesquisa>()) {
          linhaId = lPesquisas.GetById(viewModel.LinhaId).LinhaId;
        }
        ViewBag.HorarioId = new SelectList(await horarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Inicio },
            q => (q.LinhaId == linhaId) && (q.DiaId == diaId) &&
                         q.Sentido.Equals(viewModel.Sentido),
            q => q.OrderBy(h => h.Inicio)), "Id", "Name", viewModel.HorarioId);
      }
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Ponto.Prefixo }), "Id", "Name", viewModel.PontoId);
      }
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(await veiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name", viewModel.VeiculoId);
      }

      try {
        if (ModelState.IsValid) {
          Viagem viagem = mapper.Map<Viagem>(viewModel);
          await viagens.Update(viagem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Viagens/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Viagem viagem = await viagens.GetFirstAsync(v => v.Id == id);
      if (viagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ViagemViewModel>(viagem);
      return View(viewModel);
    }

    // POST: Viagens/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Viagem viagem = await viagens.GetByIdAsync(id);
      if (viagem != null) {
        await viagens.Delete(viagem);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetHorarios(int id, int? op = null, string go = null) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      using (Services<LnPesquisa> lnPesquisas = new Services<LnPesquisa>()) {
        int linhaId = lnPesquisas.GetById(id).LinhaId;
        Expression<Func<Horario, bool>> applyFilter = q => q.LinhaId == linhaId;
        if (op.HasValue) {
          applyFilter = q => (q.LinhaId == linhaId) && (q.DiaId == op.Value);
        }
        if (!string.IsNullOrWhiteSpace(go)) {
          applyFilter = q => (q.LinhaId == linhaId) && (q.DiaId == op) && q.Sentido.Equals(go);
        }

        using Services<Horario> horarios = new Services<Horario>();
        foreach (Horario item in horarios.GetQuery(applyFilter, q => q.OrderBy(h => h.Inicio))) {
          result.Add(new SelectBox() { Id = item.Id.ToString(), Name = item.Inicio.ToString(@"hh\:mm") });
        }
      }    
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (viagens != null)) {
        viagens.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
