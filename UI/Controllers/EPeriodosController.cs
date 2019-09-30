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
  public class EPeriodosController : Controller {
    private EPeriodoService ePeriodos = new EPeriodoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<EPeriodoViewModel, EPeriodo>().ReverseMap();
                                          }).CreateMapper();
    
    // GET: EPeriodos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.ePeriodos = new EPeriodoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<EPeriodoViewModel>>(await ePeriodos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: EPeriodos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EPeriodo ePeriodo = await ePeriodos.GetFirstAsync(p => p.Id == id);
      if (ePeriodo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EPeriodoViewModel>(ePeriodo);
      return View(viewModel);
    }

    // GET: EPeriodos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<Periodo> periodos = new Services<Periodo>()) {
        ViewBag.PeriodoId = new SelectList(periodos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: EPeriodos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(EPeriodoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Periodo> periodos = new Services<Periodo>()) {
        ViewBag.PeriodoId = new SelectList(await periodos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PeriodoId);
      }

      try {
        if (ModelState.IsValid) {
          EPeriodo ePeriodo = mapper.Map<EPeriodo>(viewModel);
          await ePeriodos.Insert(ePeriodo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EPeriodos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EPeriodo ePeriodo = await ePeriodos.GetByIdAsync(id);
      if (ePeriodo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EPeriodoViewModel>(ePeriodo);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Periodo> periodos = new Services<Periodo>()) {
        ViewBag.PeriodoId = new SelectList(await periodos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PeriodoId);
      }
      return View(viewModel);
    }

    // POST: EPeriodos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(EPeriodoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Periodo> periodos = new Services<Periodo>()) {
        ViewBag.PeriodoId = new SelectList(await periodos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PeriodoId);
      }

      try {
        if (ModelState.IsValid) {
          EPeriodo ePeriodo = mapper.Map<EPeriodo>(viewModel);
          await ePeriodos.Update(ePeriodo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EPeriodos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EPeriodo ePeriodo = await ePeriodos.GetFirstAsync(p => p.Id == id);
      if (ePeriodo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EPeriodoViewModel>(ePeriodo);
      return View(viewModel);
    }

    // POST: EPeriodos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      EPeriodo ePeriodo = await ePeriodos.GetByIdAsync(id);
      if (ePeriodo != null) {
        await ePeriodos.Delete(ePeriodo);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (ePeriodos != null)) {
        ePeriodos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
