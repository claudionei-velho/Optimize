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
  public class ECVeiculosController : Controller {
    private ECVeiculoService ecVeiculos = new ECVeiculoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ECVeiculoViewModel, ECVeiculo>().ReverseMap();
                                          }).CreateMapper();

    // GET: ECVeiculos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.ecVeiculos = new ECVeiculoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ECVeiculoViewModel>>(await ecVeiculos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: ECVeiculos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ECVeiculo ecVeiculo = await ecVeiculos.GetFirstAsync(d => d.Id == id);
      if (ecVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ECVeiculoViewModel>(ecVeiculo);

      return View(viewModel);
    }

    // GET: ECVeiculos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.ClasseId = new SelectList(cVeiculos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name");
      }
      return View();
    }

    // POST: ECVeiculos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ECVeiculoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.ClasseId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.ClasseId);
      }

      try {
        if (ModelState.IsValid) {
          ECVeiculo ecVeiculo = mapper.Map<ECVeiculo>(viewModel);
          await ecVeiculos.Insert(ecVeiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ECVeiculos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ECVeiculo ecVeiculo = await ecVeiculos.GetByIdAsync(id);
      if (ecVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ECVeiculoViewModel>(ecVeiculo);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.ClasseId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.ClasseId);
      }
      return View(viewModel);
    }

    // POST: ECVeiculos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ECVeiculoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.ClasseId = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.ClasseId);
      }

      try {
        if (ModelState.IsValid) {
          ECVeiculo ecVeiculo = mapper.Map<ECVeiculo>(viewModel);
          await ecVeiculos.Update(ecVeiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ECVeiculos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ECVeiculo ecVeiculo = await ecVeiculos.GetFirstAsync(d => d.Id == id);
      if (ecVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ECVeiculoViewModel>(ecVeiculo);

      return View(viewModel);
    }

    // POST: ECVeiculos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      ECVeiculo ecVeiculo = await ecVeiculos.GetByIdAsync(id);
      if (ecVeiculo != null) {
        await ecVeiculos.Delete(ecVeiculo);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (ecVeiculos != null)) {
        ecVeiculos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
