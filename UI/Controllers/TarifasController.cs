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
  public class TarifasController : Controller {
    private TarifaService tarifas = new TarifaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TarifaViewModel, Tarifa>().ReverseMap();
                                          }).CreateMapper();

    // GET: Tarifas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.tarifas = new TarifaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<TarifaViewModel>>(await tarifas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Tarifas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tarifa tarifa = await tarifas.GetFirstAsync(t => t.Id == id);
      if (tarifa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TarifaViewModel>(tarifa);
      return View(viewModel);
    }

    // GET: Tarifas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      return View();
    }

    // POST: Tarifas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TarifaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Tarifa tarifa = mapper.Map<Tarifa>(viewModel);
          await tarifas.Insert(tarifa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Tarifas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tarifa tarifa = await tarifas.GetByIdAsync(id);
      if (tarifa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TarifaViewModel>(tarifa);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      return View(viewModel);
    }

    // POST: Tarifas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TarifaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Tarifa tarifa = mapper.Map<Tarifa>(viewModel);
          await tarifas.Update(tarifa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Tarifas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tarifa tarifa = await tarifas.GetFirstAsync(t => t.Id == id);
      if (tarifa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TarifaViewModel>(tarifa);
      return View(viewModel);
    }

    // POST: Tarifas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Tarifa tarifa = await tarifas.GetByIdAsync(id);
      if (tarifa != null) {
        await tarifas.Delete(tarifa);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (tarifas != null)) {
        tarifas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
