using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class EInstalacoesController : Controller {
    private EInstalacaoService eInstalacoes = new EInstalacaoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<EInstalacaoViewModel, EInstalacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: EInstalacoes
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.eInstalacoes = new EInstalacaoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<EInstalacaoViewModel>>(await eInstalacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: EInstalacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EInstalacao eInstalacao = await eInstalacoes.GetFirstAsync(i => i.Id == id);
      if (eInstalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EInstalacaoViewModel>(eInstalacao);

      return View(viewModel);
    }

    // GET: EInstalacoes/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (InstalacaoService instalacoes = new InstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(instalacoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      using (Services<FInstalacao> funcoes = new Services<FInstalacao>()) {
        ViewBag.PropositoId = new SelectList(funcoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: EInstalacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(EInstalacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (InstalacaoService instalacoes = new InstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await instalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.InstalacaoId);
      }
      using (Services<FInstalacao> funcoes = new Services<FInstalacao>()) {
        ViewBag.PropositoId = new SelectList(await funcoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PropositoId);
      }

      try {
        if (ModelState.IsValid) {
          EInstalacao eInstalacao = mapper.Map<EInstalacao>(viewModel);
          await eInstalacoes.Insert(eInstalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EInstalacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EInstalacao eInstalacao = await eInstalacoes.GetByIdAsync(id);
      if (eInstalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EInstalacaoViewModel>(eInstalacao);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (InstalacaoService instalacoes = new InstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await instalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.InstalacaoId);
      }
      using (Services<FInstalacao> funcoes = new Services<FInstalacao>()) {
        ViewBag.PropositoId = new SelectList(await funcoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PropositoId);
      }
      return View(viewModel);
    }

    // POST: EInstalacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(EInstalacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (InstalacaoService instalacoes = new InstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await instalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.InstalacaoId);
      }
      using (Services<FInstalacao> funcoes = new Services<FInstalacao>()) {
        ViewBag.PropositoId = new SelectList(await funcoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PropositoId);
      }

      try {
        if (ModelState.IsValid) {
          EInstalacao eInstalacao = mapper.Map<EInstalacao>(viewModel);
          await eInstalacoes.Update(eInstalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EInstalacoes/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EInstalacao eInstalacao = await eInstalacoes.GetFirstAsync(i => i.Id == id);
      if (eInstalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EInstalacaoViewModel>(eInstalacao);

      return View(viewModel);
    }

    // POST: EInstalacoes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      EInstalacao eInstalacao = await eInstalacoes.GetByIdAsync(id);
      if (eInstalacao != null) {
        await eInstalacoes.Delete(eInstalacao);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (eInstalacoes != null)) {
        eInstalacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
