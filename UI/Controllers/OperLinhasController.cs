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
  public class OperLinhasController : Controller {
    private readonly Services<OperLinha> operLinhas = new Services<OperLinha>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<OperLinhaViewModel, OperLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: OperLinhas
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<OperLinhaViewModel>>(await operLinhas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: OperLinhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      OperLinha operLinha = await operLinhas.GetByIdAsync(id);
      if (operLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OperLinhaViewModel>(operLinha);
      return View(viewModel);
    }

    // GET: OperLinhas/Create
    public ActionResult Create() {
      return View();
    }

    // POST: OperLinhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(OperLinhaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          OperLinha operLinha = mapper.Map<OperLinha>(viewModel);
          await operLinhas.Insert(operLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: OperLinhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      OperLinha operLinha = await operLinhas.GetByIdAsync(id);
      if (operLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OperLinhaViewModel>(operLinha);
      return View(viewModel);
    }

    // POST: OperLinhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(OperLinhaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          OperLinha operLinha = mapper.Map<OperLinha>(viewModel);
          await operLinhas.Update(operLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (operLinhas != null)) {
        operLinhas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
