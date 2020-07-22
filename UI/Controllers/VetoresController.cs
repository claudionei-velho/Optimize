using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Services;
using Dto.Models;
using UI.Models;

namespace UI.Controllers {
  public class VetoresController : Controller {
    private readonly VetorService vetores = new VetorService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                              cfg.CreateMap<VetorViewModel, Vetor>().ReverseMap();
                                          }).CreateMapper();

    // GET: Vetores
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<VetorViewModel>>(await vetores.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Vetores/Details/5
    public ActionResult Details(int id) {
      return View();
    }

    // GET: Vetores/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Vetores/Create
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

    // GET: Vetores/Edit/5
    public async Task<ActionResult> Edit(int id) {
      using (MatrizHService matrizes = new MatrizHService()) {
        await matrizes.Vectorize(id);
      }
      return RedirectToAction("Index");
    }

    // POST: Vetores/Edit/5
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

    // GET: Vetores/Delete/5
    public ActionResult Delete(int id) {
      return View();
    }

    // POST: Vetores/Delete/5
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
