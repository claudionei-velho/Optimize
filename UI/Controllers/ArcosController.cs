using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;

using Bll.Services;

using Dto.Models;

using PagedList;

using UI.Models;

namespace UI.Controllers {
  public class ArcosController : Controller {
    private readonly ArcoService arcos = new ArcoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                              cfg.CreateMap<ArcoViewModel, Arco>().ReverseMap();
                                          }).CreateMapper();

    // GET: Arcos
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<ArcoViewModel>>(await arcos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Arcos/Details/5
    public ActionResult Details(int id) {
      return View();
    }

    // GET: Arcos/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Arcos/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection) {
      try {
        // TODO: Add insert logic here

        return RedirectToAction("Index");
      }
      catch {
        return View();
      }
    }

    // GET: Arcos/Edit/5
    public async Task<ActionResult> Edit(int id) {
      using (VetorService vetores = new VetorService()) {
        await vetores.DoArcs(id);
      }
      return RedirectToAction("Index");
    }

    // POST: Arcos/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection) {
      try {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch {
        return View();
      }
    }

    // GET: Arcos/Delete/5
    public ActionResult Delete(int id) {
      return View();
    }

    // POST: Arcos/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection) {
      try {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch {
        return View();
      }
    }
  }
}
