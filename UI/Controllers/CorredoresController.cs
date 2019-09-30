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
  public class CorredoresController : Controller {
    private CorredorService corredores = new CorredorService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<CorredorViewModel, Corredor>().ReverseMap();
                                          }).CreateMapper();

    // GET: Corredores
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.corredores = new CorredorService(user.ID);

      var viewModel = mapper.Map<IEnumerable<CorredorViewModel>>(await corredores.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Corredores/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Corredor corredor = await corredores.GetFirstAsync(c => c.Id == id);
      if (corredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CorredorViewModel>(corredor);
      return View(viewModel);
    }

    // GET: Corredores/Create
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

    // POST: Corredores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CorredorViewModel viewModel) {
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
          Corredor corredor = mapper.Map<Corredor>(viewModel);
          await corredores.Insert(corredor);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Corredores/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Corredor corredor = await corredores.GetByIdAsync(id);
      if (corredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CorredorViewModel>(corredor);

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

    // POST: Corredores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CorredorViewModel viewModel) {
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
          Corredor corredor = mapper.Map<Corredor>(viewModel);
          await corredores.Update(corredor);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Corredores/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Corredor corredor = await corredores.GetFirstAsync(c => c.Id == id);
      if (corredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CorredorViewModel>(corredor);
      return View(viewModel);
    }

    // POST: Corredores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Corredor corredor = await corredores.GetByIdAsync(id);
      if (corredor != null) {
        await corredores.Delete(corredor);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (corredores != null)) {
        corredores.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
