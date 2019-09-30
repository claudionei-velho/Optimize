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
  public class CLinhasController : Controller {
    private CLinhaService cLinhas = new CLinhaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<CLinhaViewModel, CLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: CLinhas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.cLinhas = new CLinhaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<CLinhaViewModel>>(await cLinhas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: CLinhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CLinha cLinha = await cLinhas.GetFirstAsync(c => c.Id == id);
      if (cLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CLinhaViewModel>(cLinha);
      return View(viewModel);
    }

    // GET: CLinhas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<ClassLinha> classLinhas = new Services<ClassLinha>()) {
        ViewBag.ClassLinhaId = new SelectList(classLinhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: CLinhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<ClassLinha> classLinhas = new Services<ClassLinha>()) {
        ViewBag.ClassLinhaId = new SelectList(await classLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.ClassLinhaId);
      }

      try {
        if (ModelState.IsValid) {
          CLinha cLinha = mapper.Map<CLinha>(viewModel);
          await cLinhas.Insert(cLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: CLinhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CLinha cLinha = await cLinhas.GetByIdAsync(id);
      if (cLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CLinhaViewModel>(cLinha);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<ClassLinha> classLinhas = new Services<ClassLinha>()) {
        ViewBag.ClassLinhaId = new SelectList(await classLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.ClassLinhaId);
      }
      return View(viewModel);
    }

    // POST: CLinhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<ClassLinha> classLinhas = new Services<ClassLinha>()) {
        ViewBag.ClassLinhaId = new SelectList(await classLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.ClassLinhaId);
      }

      try {
        if (ModelState.IsValid) {
          CLinha cLinha = mapper.Map<CLinha>(viewModel);
          await cLinhas.Update(cLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: CLinhas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CLinha cLinha = await cLinhas.GetFirstAsync(c => c.Id == id);
      if (cLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CLinhaViewModel>(cLinha);
      return View(viewModel);
    }

    // POST: CLinhas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      CLinha cLinha = await cLinhas.GetByIdAsync(id);
      if (cLinha != null) {
        await cLinhas.Delete(cLinha);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (cLinhas != null)) {
        cLinhas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
