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
  public class CustosController : Controller {
    private CustoService custos = new CustoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<CustoViewModel, Custo>().ReverseMap();
                                          }).CreateMapper();

    // GET: Custos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.custos = new CustoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<CustoViewModel>>(await custos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Custos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Custo custo = await custos.GetFirstAsync(c => c.Id == id);
      if (custo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CustoViewModel>(custo);
      return View(viewModel);
    }

    // GET: Custos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      return View();
    }

    // POST: Custos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CustoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Custo custo = mapper.Map<Custo>(viewModel);
          await custos.Insert(custo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Custos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Custo custo = await custos.GetByIdAsync(id);
      if (custo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CustoViewModel>(custo);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      return View(viewModel);
    }

    // POST: Custos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CustoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          Custo custo = mapper.Map<Custo>(viewModel);
          await custos.Update(custo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Custos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Custo custo = await custos.GetFirstAsync(c => c.Id == id);
      if (custo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CustoViewModel>(custo);
      return View(viewModel);
    }

    // POST: Custos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Custo custo = await custos.GetByIdAsync(id);
      if (custo != null) {
        await custos.Delete(custo);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (custos != null)) {
        custos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
