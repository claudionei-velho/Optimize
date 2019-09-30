using System.Collections.Generic;
using System.Linq;
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
  public class PaisesController : Controller {
    private readonly Services<Pais> paises = new Services<Pais>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PaisViewModel, Pais>().ReverseMap();
                                          }).CreateMapper();

    // GET: Paises
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<PaisViewModel>>(
                          await paises.GetAllAsync(orderBy: q => q.OrderBy(e => e.Nome)));
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Paises/Details/5
    public async Task<ActionResult> Details(string id) {
      if (string.IsNullOrWhiteSpace(id)) { 
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Pais pais = await paises.GetByIdAsync(id);
      if (pais == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PaisViewModel>(pais);
      return View(viewModel);
    }

    // GET: Paises/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Paises/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PaisViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          var pais = mapper.Map<Pais>(viewModel);
          await paises.Insert(pais);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Paises/Edit/5
    public async Task<ActionResult> Edit(string id) {
      if (string.IsNullOrWhiteSpace(id)) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Pais pais = await paises.GetByIdAsync(id);
      if (pais == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PaisViewModel>(pais);
      return View(viewModel);
    }

    // POST: Paises/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PaisViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          var pais = mapper.Map<Pais>(viewModel);
          await paises.Update(pais);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (paises != null)) {
        paises.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
