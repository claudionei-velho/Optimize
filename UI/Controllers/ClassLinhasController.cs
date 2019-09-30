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
  public class ClassLinhasController : Controller {
    private readonly Services<ClassLinha> classLinhas = new Services<ClassLinha>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ClassLinhaViewModel, ClassLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: ClassLinhas
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<ClassLinhaViewModel>>(await classLinhas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: ClassLinhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ClassLinha classLinha = await classLinhas.GetByIdAsync(id);
      if (classLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ClassLinhaViewModel>(classLinha);
      return View(viewModel);
    }

    // GET: ClassLinhas/Create
    public ActionResult Create() {
      return View();
    }

    // POST: ClassLinhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ClassLinhaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          ClassLinha classLinha = mapper.Map<ClassLinha>(viewModel);
          await classLinhas.Insert(classLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ClassLinhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ClassLinha classLinha = await classLinhas.GetByIdAsync(id);
      if (classLinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ClassLinhaViewModel>(classLinha);
      return View(viewModel);
    }

    // POST: ClassLinhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ClassLinhaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          ClassLinha classLinha = mapper.Map<ClassLinha>(viewModel);
          await classLinhas.Update(classLinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (classLinhas != null)) {
        classLinhas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
