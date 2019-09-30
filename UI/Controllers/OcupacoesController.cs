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
  public class OcupacoesController : Controller {
    private readonly Services<Ocupacao> ocupacoes = new Services<Ocupacao>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<OcupacaoViewModel, Ocupacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: Ocupacoes
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<OcupacaoViewModel>>(await ocupacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Ocupacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ocupacao ocupacao = await ocupacoes.GetByIdAsync(id);
      if (ocupacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OcupacaoViewModel>(ocupacao);
      return View(viewModel);
    }

    // GET: Ocupacoes/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Ocupacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(OcupacaoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Ocupacao ocupacao = mapper.Map<Ocupacao>(viewModel);
          await ocupacoes.Insert(ocupacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Ocupacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ocupacao ocupacao = await ocupacoes.GetByIdAsync(id);
      if (ocupacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OcupacaoViewModel>(ocupacao);
      return View(viewModel);
    }

    // POST: Ocupacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(OcupacaoViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Ocupacao ocupacao = mapper.Map<Ocupacao>(viewModel);
          await ocupacoes.Update(ocupacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (ocupacoes != null)) {
        ocupacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
