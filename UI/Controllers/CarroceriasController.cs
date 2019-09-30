using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class CarroceriasController : Controller {
    private CarroceriaService carrocerias = new CarroceriaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<CarroceriaViewModel, Carroceria>().ReverseMap();
                                          }).CreateMapper();

    // GET: Carrocerias
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.carrocerias = new CarroceriaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<CarroceriaViewModel>>(await carrocerias.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Carrocerias/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Carroceria carroceria = await carrocerias.GetFirstAsync(c => c.VeiculoId == id);
      if (carroceria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CarroceriaViewModel>(carroceria);
      return View(viewModel);
    }

    // GET: Carrocerias/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(veiculos.AddCarrocerias(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name");
      }
      ViewBag.Catraca = new SelectList(new Posicao().GetAll(), "Id", "Name");
      ViewBag.PortaIn = new SelectList(new Posicao().GetAll(), "Id", "Name");

      return View();
    }

    // POST: Carrocerias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CarroceriaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(await veiculos.AddCarroceriasAsync(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name", viewModel.VeiculoId);
      }
      ViewBag.Catraca = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.Catraca);
      ViewBag.PortaIn = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PortaIn);

      try {
        if (ModelState.IsValid) {
          Carroceria carroceria = mapper.Map<Carroceria>(viewModel);
          await carrocerias.Insert(carroceria);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Carrocerias/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Carroceria carroceria = await carrocerias.GetByIdAsync(id);
      if (carroceria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CarroceriaViewModel>(carroceria);

      ViewBag.Catraca = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.Catraca);
      ViewBag.PortaIn = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PortaIn);

      return View(viewModel);
    }

    // POST: Carrocerias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CarroceriaViewModel viewModel) {
      ViewBag.Catraca = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.Catraca);
      ViewBag.PortaIn = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PortaIn);

      try {
        if (ModelState.IsValid) {
          Carroceria carroceria = mapper.Map<Carroceria>(viewModel);
          await carrocerias.Update(carroceria);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Carrocerias/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Carroceria carroceria = await carrocerias.GetFirstAsync(c => c.VeiculoId == id);
      if (carroceria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<CarroceriaViewModel>(carroceria);
      return View(viewModel);
    }

    // POST: Carrocerias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Carroceria carroceria = await carrocerias.GetByIdAsync(id);
      if (carroceria != null) {
        await carrocerias.Delete(carroceria);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (carrocerias != null)) {
        carrocerias.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
