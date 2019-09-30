using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class LnPesquisasController : Controller {
    private LnPesquisaService lPesquisas = new LnPesquisaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<LnPesquisaViewModel, LnPesquisa>().ReverseMap();
                                          }).CreateMapper();

    // GET: LnPesquisas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lPesquisas = new LnPesquisaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<LnPesquisaViewModel>>(await lPesquisas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: LnPesquisas
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<LnPesquisaViewModel>>(
                          await lPesquisas.GetAllAsync(q => q.PesquisaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: LnPesquisas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnPesquisa lPesquisa = await lPesquisas.GetFirstAsync(l => l.Id == id);
      if (lPesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnPesquisaViewModel>(lPesquisa);
      return View(viewModel);
    }

    // GET: LnPesquisas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (PesquisaService pesquisas = new PesquisaService(user.ID)) {
        ViewBag.PesquisaId = new SelectList(pesquisas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Identificacao }), "Id", "Name");
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      return View();
    }

    // POST: LnPesquisas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LnPesquisaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (PesquisaService pesquisas = new PesquisaService(user.ID)) {
        ViewBag.PesquisaId = new SelectList(await pesquisas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Identificacao }), "Id", "Name", viewModel.PesquisaId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          LnPesquisa lPesquisa = mapper.Map<LnPesquisa>(viewModel);
          await lPesquisas.Insert(lPesquisa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnPesquisas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnPesquisa lPesquisa = await lPesquisas.GetByIdAsync(id);
      if (lPesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnPesquisaViewModel>(lPesquisa);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (PesquisaService pesquisas = new PesquisaService(user.ID)) {
        ViewBag.PesquisaId = new SelectList(await pesquisas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Identificacao }), "Id", "Name", viewModel.PesquisaId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      return View(viewModel);
    }

    // POST: LnPesquisas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LnPesquisaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (PesquisaService pesquisas = new PesquisaService(user.ID)) {
        ViewBag.PesquisaId = new SelectList(await pesquisas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Identificacao }), "Id", "Name", viewModel.PesquisaId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          LnPesquisa lPesquisa = mapper.Map<LnPesquisa>(viewModel);
          await lPesquisas.Update(lPesquisa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnPesquisas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnPesquisa lPesquisa = await lPesquisas.GetFirstAsync(l => l.Id == id);
      if (lPesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnPesquisaViewModel>(lPesquisa);
      return View(viewModel);
    }

    // POST: LnPesquisas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      LnPesquisa lPesquisa = await lPesquisas.GetByIdAsync(id);
      if (lPesquisa != null) {
        await lPesquisas.Delete(lPesquisa);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lPesquisas != null)) {
        lPesquisas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
