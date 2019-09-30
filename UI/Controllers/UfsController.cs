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
  public class UfsController : Controller {
    private readonly Services<Uf> ufs = new Services<Uf>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<UfViewModel, Uf>().ReverseMap();
                                          }).CreateMapper();

    // GET: Ufs
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<UfViewModel>>(
                          await ufs.GetAllAsync(orderBy: q => q.OrderBy(e => e.Estado)));
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Ufs/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Uf uf = await ufs.GetByIdAsync(id);
      if (uf == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<UfViewModel>(uf);
      return View(viewModel);
    }

    // GET: Ufs/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Ufs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(UfViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          var uf = mapper.Map<Uf>(viewModel);
          await ufs.Insert(uf);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Ufs/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Uf uf = await ufs.GetByIdAsync(id);
      if (uf == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<UfViewModel>(uf);
      return View(viewModel);
    }

    // POST: Ufs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UfViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          var uf = mapper.Map<Uf>(viewModel);
          await ufs.Update(uf);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (ufs != null)) {
        ufs.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
