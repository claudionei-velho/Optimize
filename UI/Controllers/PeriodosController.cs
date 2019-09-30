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
  public class PeriodosController : Controller {
    private readonly Services<Periodo> periodos = new Services<Periodo>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PeriodoViewModel, Periodo>().ReverseMap();
                                          }).CreateMapper();

    // GET: Periodos
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<PeriodoViewModel>>(await periodos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Periodos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Periodo periodo = await periodos.GetByIdAsync(id);
      if (periodo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PeriodoViewModel>(periodo);
      return View(viewModel);
    }

    // GET: Periodos/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Periodos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PeriodoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Periodo periodo = mapper.Map<Periodo>(viewModel);
          await periodos.Insert(periodo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Periodos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Periodo periodo = await periodos.GetByIdAsync(id);
      if (periodo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PeriodoViewModel>(periodo);
      return View(viewModel);
    }

    // POST: Periodos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PeriodoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Periodo periodo = mapper.Map<Periodo>(viewModel);
          await periodos.Update(periodo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    public JsonResult GetPeriodos(int id) {
      return Json(periodos.GetById(id).Denominacao, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (periodos != null)) {
        periodos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
