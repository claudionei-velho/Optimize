using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class ChassisController : Controller {
    private ChassiService chassis = new ChassiService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ChassiViewModel, Chassi>().ReverseMap();
                                          }).CreateMapper();

    // GET: Chassis
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.chassis = new ChassiService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ChassiViewModel>>(await chassis.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Chassis/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Chassi chassi = await chassis.GetFirstAsync(c => c.VeiculoId == id);
      if (chassi == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ChassiViewModel>(chassi);
      return View(viewModel);
    }

    // GET: Chassis/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(veiculos.AddChassis(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name");
      }
      using (Services<Motor> motores = new Services<Motor>()) {
        ViewBag.MotorId = new SelectList(motores.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      ViewBag.PosMotor = new SelectList(new Posicao().GetAll(), "Id", "Name");
      ViewBag.TransmiteId = new SelectList(new Transmissao().GetAll(), "Id", "Name");
      ViewBag.DirecaoId = new SelectList(new Direcao().GetAll(), "Id", "Name");

      return View();
    }

    // POST: Chassis/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ChassiViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (VeiculoService veiculos = new VeiculoService(user.ID)) {
        ViewBag.VeiculoId = new SelectList(await veiculos.AddChassisAsync(
            q => new { Id = q.Id.ToString(), Name = q.Numero }), "Id", "Name", viewModel.VeiculoId);
      }
      using (Services<Motor> motores = new Services<Motor>()) {
        ViewBag.MotorId = new SelectList(await motores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.MotorId);
      }
      ViewBag.PosMotor = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PosMotor);
      ViewBag.TransmiteId = new SelectList(new Transmissao().GetAll(), "Id", "Name", viewModel.TransmiteId);
      ViewBag.DirecaoId = new SelectList(new Direcao().GetAll(), "Id", "Name", viewModel.DirecaoId);

      try {
        if (ModelState.IsValid) {
          Chassi chassi = mapper.Map<Chassi>(viewModel);
          await chassis.Insert(chassi);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Chassis/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Chassi chassi = await chassis.GetByIdAsync(id);
      if (chassi == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ChassiViewModel>(chassi);

      using (Services<Motor> motores = new Services<Motor>()) {
        ViewBag.MotorId = new SelectList(await motores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.MotorId);
      }
      ViewBag.PosMotor = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PosMotor);
      ViewBag.TransmiteId = new SelectList(new Transmissao().GetAll(), "Id", "Name", viewModel.TransmiteId);
      ViewBag.DirecaoId = new SelectList(new Direcao().GetAll(), "Id", "Name", viewModel.DirecaoId);

      return View(viewModel);
    }

    // POST: Chassis/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ChassiViewModel viewModel) {
      using (Services<Motor> motores = new Services<Motor>()) {
        ViewBag.MotorId = new SelectList(await motores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.MotorId);
      }
      ViewBag.PosMotor = new SelectList(new Posicao().GetAll(), "Id", "Name", viewModel.PosMotor);
      ViewBag.TransmiteId = new SelectList(new Transmissao().GetAll(), "Id", "Name", viewModel.TransmiteId);
      ViewBag.DirecaoId = new SelectList(new Direcao().GetAll(), "Id", "Name", viewModel.DirecaoId);

      try {
        if (ModelState.IsValid) {
          Chassi chassi = mapper.Map<Chassi>(viewModel);
          await chassis.Update(chassi);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Chassis/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Chassi chassi = await chassis.GetFirstAsync(c => c.VeiculoId == id);
      if (chassi == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ChassiViewModel>(chassi);
      return View(viewModel);
    }

    // POST: Chassis/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Chassi chassi = await chassis.GetByIdAsync(id);
      if (chassi != null) {
        await chassis.Delete(chassi);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (chassis != null)) {
        chassis.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
