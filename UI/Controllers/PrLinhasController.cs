using System.Collections.Generic;
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
  public class PrLinhasController : Controller {
    private PrLinhaService lPeriodos = new PrLinhaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PrLinhaViewModel, PrLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: PrLinhas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lPeriodos = new PrLinhaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PrLinhaViewModel>>(await lPeriodos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: PrLinhas
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<PrLinhaViewModel>>(
                          await lPeriodos.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: PrLinhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PrLinha prLinha = await lPeriodos.GetFirstAsync(p => p.Id == id);
      if (prLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PrLinhaViewModel>(prLinha);
      return View(viewModel);
    }

    // GET: PrLinhas/Create
    public ActionResult Create(int? id) {
      var viewModel = new PrLinhaViewModel {
        LinhaId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      using (EPeriodoService ePeriodos = new EPeriodoService(user.ID)) {
        ViewBag.PeriodoId = new SelectList(ePeriodos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Empresa.Fantasia + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name");
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.CVeiculoId = new SelectList(cVeiculos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name");
      }
      using (Services<Ocupacao> ocupacoes = new Services<Ocupacao>()) {
        ViewBag.OcupacaoId = new SelectList(ocupacoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: PrLinhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PrLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      using (EPeriodoService ePeriodos = new EPeriodoService(user.ID)) {
        ViewBag.PeriodoId = new SelectList(await ePeriodos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Empresa.Fantasia + " | " + q.Denominacao
            }), "Id", "Name", viewModel.PeriodoId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.CVeiculoId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.CVeiculoId);
      }
      using (Services<Ocupacao> ocupacoes = new Services<Ocupacao>()) {
        ViewBag.OcupacaoId = new SelectList(await ocupacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OcupacaoId);
      }

      try {
        if (ModelState.IsValid) {
          PrLinha prLinha = mapper.Map<PrLinha>(viewModel);
          await lPeriodos.Insert(prLinha);
        }
        return RedirectToAction("Filter", new { id = viewModel.LinhaId });
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PrLinhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PrLinha prLinha = await lPeriodos.GetByIdAsync(id);
      if (prLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PrLinhaViewModel>(prLinha);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      using (EPeriodoService ePeriodos = new EPeriodoService(user.ID)) {
        ViewBag.PeriodoId = new SelectList(await ePeriodos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Empresa.Fantasia + " | " + q.Denominacao
            }), "Id", "Name", viewModel.PeriodoId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.CVeiculoId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.CVeiculoId);
      }
      using (Services<Ocupacao> ocupacoes = new Services<Ocupacao>()) {
        ViewBag.OcupacaoId = new SelectList(await ocupacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OcupacaoId);
      }
      return View(viewModel);
    }

    // POST: PrLinhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PrLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      using (EPeriodoService ePeriodos = new EPeriodoService(user.ID)) {
        ViewBag.PeriodoId = new SelectList(await ePeriodos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Empresa.Fantasia + " | " + q.Denominacao
            }), "Id", "Name", viewModel.PeriodoId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.CVeiculoId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.CVeiculoId);
      }
      using (Services<Ocupacao> ocupacoes = new Services<Ocupacao>()) {
        ViewBag.OcupacaoId = new SelectList(await ocupacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OcupacaoId);
      }

      try {
        if (ModelState.IsValid) {
          PrLinha prLinha = mapper.Map<PrLinha>(viewModel);
          await lPeriodos.Update(prLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PrLinhas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PrLinha prLinha = await lPeriodos.GetFirstAsync(p => p.Id == id);
      if (prLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PrLinhaViewModel>(prLinha);
      return View(viewModel);
    }

    // POST: PrLinhas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      PrLinha prLinha = await lPeriodos.GetByIdAsync(id);
      if (prLinha != null) {
        await lPeriodos.Delete(prLinha);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lPeriodos != null)) {
        lPeriodos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
