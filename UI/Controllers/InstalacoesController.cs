using System.Collections.Generic;
using System.Linq;
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
  public class InstalacoesController : Controller {
    private InstalacaoService instalacoes = new InstalacaoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<InstalacaoViewModel, Instalacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: Instalacoes
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.instalacoes = new InstalacaoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<InstalacaoViewModel>>(await instalacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Instalacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Instalacao instalacao = await instalacoes.GetFirstAsync(i => i.Id == id);
      if (instalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<InstalacaoViewModel>(instalacao);

      return View(viewModel);
    }

    // GET: Instalacoes/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(ufs.GetSelect(
            q => new { Id = q.Sigla, Name = q.Estado },
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name");
      }
      return View();
    }

    // POST: Instalacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(InstalacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado },
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }

      try {
        if (ModelState.IsValid) {
          Instalacao instalacao = mapper.Map<Instalacao>(viewModel);
          await instalacoes.Insert(instalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Instalacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Instalacao instalacao = await instalacoes.GetByIdAsync(id);
      if (instalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<InstalacaoViewModel>(instalacao);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado },
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }
      return View(viewModel);
    }

    // POST: Instalacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(InstalacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado },
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }

      try {
        if (ModelState.IsValid) {
          Instalacao instalacao = mapper.Map<Instalacao>(viewModel);
          await instalacoes.Update(instalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Instalacoes/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Instalacao instalacao = await instalacoes.GetFirstAsync(i => i.Id == id);
      if (instalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<InstalacaoViewModel>(instalacao);

      return View(viewModel);
    }

    // POST: Instalacoes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Instalacao instalacao = await instalacoes.GetByIdAsync(id);
      if (instalacao != null) {
        await instalacoes.Delete(instalacao);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (instalacoes != null)) {
        instalacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
