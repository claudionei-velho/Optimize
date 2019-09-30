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
  public class EDominiosController : Controller {
    private EDominioService eDominios = new EDominioService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<EDominioViewModel, EDominio>().ReverseMap();
                                          }).CreateMapper();

    // GET: EDominios
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.eDominios = new EDominioService(user.ID);

      var viewModel = mapper.Map<IEnumerable<EDominioViewModel>>(await eDominios.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: EDominios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EDominio eDominio = await eDominios.GetFirstAsync(d => d.Id == id);
      if (eDominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EDominioViewModel>(eDominio);
      return View(viewModel);
    }

    // GET: EDominios/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<Dominio> dominios = new Services<Dominio>()) {
        ViewBag.DominioId = new SelectList(dominios.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: EDominios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(EDominioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Dominio> dominios = new Services<Dominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.DominioId);
      }

      try {
        if (ModelState.IsValid) {
          EDominio eDominio = mapper.Map<EDominio>(viewModel);
          await eDominios.Insert(eDominio);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EDominios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EDominio eDominio = await eDominios.GetByIdAsync(id);
      if (eDominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EDominioViewModel>(eDominio);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Dominio> dominios = new Services<Dominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.DominioId);
      }
      return View(viewModel);
    }

    // POST: EDominios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(EDominioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Dominio> dominios = new Services<Dominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.DominioId);
      }

      try {
        if (ModelState.IsValid) {
          EDominio eDominio = mapper.Map<EDominio>(viewModel);
          await eDominios.Update(eDominio);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EDominios/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EDominio eDominio = await eDominios.GetFirstAsync(d => d.Id == id);
      if (eDominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EDominioViewModel>(eDominio);
      return View(viewModel);
    }

    // POST: EDominios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      EDominio eDominio = await eDominios.GetByIdAsync(id);
      if (eDominio != null) {
        await eDominios.Delete(eDominio);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (eDominios != null)) {
        eDominios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
