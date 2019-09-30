using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Dto.Models;
using UI.Models;

namespace UI.Controllers {
  [Authorize]
  public class CVeiculosController : Controller {
    private readonly Services<CVeiculo> cVeiculos = new Services<CVeiculo>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<CVeiculoViewModel, CVeiculo>().ReverseMap();
                                          }).CreateMapper();

    // GET: CVeiculos
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<CVeiculoViewModel>>(await cVeiculos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: CVeiculos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CVeiculo cVeiculo = await cVeiculos.GetByIdAsync(id);
      if (cVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CVeiculoViewModel>(cVeiculo);
      return View(viewModel);
    }

    // GET: CVeiculos/Create
    public ActionResult Create() {
      return View();
    }

    // POST: CVeiculos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CVeiculoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          CVeiculo cVeiculo = mapper.Map<CVeiculo>(viewModel);
          await cVeiculos.Insert(cVeiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: CVeiculos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CVeiculo cVeiculo = await cVeiculos.GetByIdAsync(id);
      if (cVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CVeiculoViewModel>(cVeiculo);
      return View(viewModel);
    }

    // POST: CVeiculos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CVeiculoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          CVeiculo cVeiculo = mapper.Map<CVeiculo>(viewModel);
          await cVeiculos.Update(cVeiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: CVeiculos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      CVeiculo cVeiculo = await cVeiculos.GetByIdAsync(id);
      if (cVeiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CVeiculoViewModel>(cVeiculo);
      return View(viewModel);
    }

    // POST: CVeiculos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      CVeiculo cVeiculo = await cVeiculos.GetByIdAsync(id);
      if (cVeiculo != null) {
        await cVeiculos.Delete(cVeiculo);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetClasseVeiculo(int id) {      
      return Json(mapper.Map<CVeiculoViewModel>(cVeiculos.GetById(id)), JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (cVeiculos != null)) {
        cVeiculos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
