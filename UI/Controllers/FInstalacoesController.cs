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
  public class FInstalacoesController : Controller {
    private readonly Services<FInstalacao> fInstalacoes = new Services<FInstalacao>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<FInstalacaoViewModel, FInstalacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: FInstalacoes
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<FInstalacaoViewModel>>(await fInstalacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: FInstalacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FInstalacao fInstalacao = await fInstalacoes.GetByIdAsync(id);
      if (fInstalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FInstalacaoViewModel>(fInstalacao);

      return View(viewModel);
    }

    // GET: FInstalacoes/Create
    public ActionResult Create() {
      return View();
    }

    // POST: FInstalacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(FInstalacaoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          FInstalacao fInstalacao = mapper.Map<FInstalacao>(viewModel);
          await fInstalacoes.Insert(fInstalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: FInstalacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FInstalacao fInstalacao = await fInstalacoes.GetByIdAsync(id);
      if (fInstalacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FInstalacaoViewModel>(fInstalacao);

      return View(viewModel);
    }

    // POST: FInstalacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(FInstalacaoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          FInstalacao fInstalacao = mapper.Map<FInstalacao>(viewModel);
          await fInstalacoes.Update(fInstalacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (fInstalacoes != null)) {
        fInstalacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
