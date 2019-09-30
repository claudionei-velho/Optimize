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
  public class TroncosController : Controller {
    private TroncoService troncos = new TroncoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TroncoViewModel, Tronco>().ReverseMap();
                                          }).CreateMapper();

    // GET: Troncos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.troncos = new TroncoService(user.ID);
     
      var viewModel = mapper.Map<IEnumerable<TroncoViewModel>>(await troncos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Troncos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tronco tronco = await troncos.GetFirstAsync(t => t.Id == id);
      if (tronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TroncoViewModel>(tronco);
      return View(viewModel);
    }

    // GET: Troncos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      return View();
    }

    // POST: Troncos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Tronco tronco = mapper.Map<Tronco>(viewModel);
          await troncos.Insert(tronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Troncos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tronco tronco = await troncos.GetByIdAsync(id);
      if (tronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TroncoViewModel>(tronco);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      return View(viewModel);
    }

    // POST: Troncos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Tronco tronco = mapper.Map<Tronco>(viewModel);
          await troncos.Update(tronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Troncos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Tronco tronco = await troncos.GetFirstAsync(t => t.Id == id);
      if (tronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TroncoViewModel>(tronco);
      return View(viewModel);
    }

    // POST: Troncos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Tronco tronco = await troncos.GetByIdAsync(id);
      if (tronco != null) {
        await troncos.Delete(tronco);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (troncos != null)) {
        troncos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
