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
  public class EmpresasController : Controller {
    private EmpresaService empresas = new EmpresaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                        cfg.CreateMap<EmpresaViewModel, Empresa>().ReverseMap();
                                      }).CreateMapper();

    // GET: Empresas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.empresas = new EmpresaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<EmpresaViewModel>>(await empresas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Empresas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Empresa empresa = await empresas.GetFirstAsync(e => e.Id == id);
      if (empresa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EmpresaViewModel>(empresa);
      return View(viewModel);
    }

    // GET: Empresas/Create
    public ActionResult Create() {
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(ufs.GetSelect(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name");
      }
      using (Services<Pais> paises = new Services<Pais>()) {
        ViewBag.PaisId = new SelectList(paises.GetSelect(
            q => new { q.Id, Name = q.Nome }, 
            orderBy: q => q.OrderBy(p => p.Nome)), "Id", "Name");
      }
      return View();
    }

    // POST: Empresas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(EmpresaViewModel viewModel) {
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }
      using (Services<Pais> paises = new Services<Pais>()) {
        ViewBag.PaisId = new SelectList(await paises.GetSelectAsync(
            q => new { q.Id, Name = q.Nome }, 
            orderBy: q => q.OrderBy(p => p.Nome)), "Id", "Name", viewModel.PaisId);
      }

      try {
        if (ModelState.IsValid) {
          Empresa empresa = mapper.Map<Empresa>(viewModel);
          await empresas.Insert(empresa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Empresas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Empresa empresa = await empresas.GetByIdAsync(id);
      if (empresa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EmpresaViewModel>(empresa);

      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }
      using (Services<Pais> paises = new Services<Pais>()) {
        ViewBag.PaisId = new SelectList(await paises.GetSelectAsync(
            q => new { q.Id, Name = q.Nome }, 
            orderBy: q => q.OrderBy(p => p.Nome)), "Id", "Name", viewModel.PaisId);
      }
      return View(viewModel);
    }

    // POST: Empresas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(EmpresaViewModel viewModel) {
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }
      using (Services<Pais> paises = new Services<Pais>()) {
        ViewBag.PaisId = new SelectList(await paises.GetSelectAsync(
            q => new { q.Id, Name = q.Nome }, 
            orderBy: q => q.OrderBy(p => p.Nome)), "Id", "Name", viewModel.PaisId);
      }

      try {
        if (ModelState.IsValid) {
          Empresa empresa = mapper.Map<Empresa>(viewModel);
          await empresas.Update(empresa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Empresas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Empresa empresa = await empresas.GetFirstAsync(e => e.Id == id);
      if (empresa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EmpresaViewModel>(empresa);
      return View(viewModel);
    }

    // POST: Empresas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Empresa empresa = await empresas.GetByIdAsync(id);
      if (empresa != null) {
        await empresas.Delete(empresa);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (empresas != null)) {
        empresas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
