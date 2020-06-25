using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Services;
using Dto.Lists;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class VeiculosController : Controller {
    private VeiculoService veiculos = new VeiculoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<VeiculoViewModel, Veiculo>().ReverseMap();
                                          }).CreateMapper();

    // GET: Veiculos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.veiculos = new VeiculoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<VeiculoViewModel>>(await veiculos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Veiculos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.Classe = new SelectList(cVeiculos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name");
      }
      ViewBag.Categoria = new SelectList(Categoria.Items.Where(p => p.Key > 0).ToList(), "Key", "Value");

      return View();
    }

    // POST: Veiculos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(VeiculoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.Classe = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.Classe);
      }
      ViewBag.Categoria = new SelectList(Categoria.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.Categoria);

      try {
        if (ModelState.IsValid) {
          Veiculo veiculo = mapper.Map<Veiculo>(viewModel);
          await veiculos.Insert(veiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Veiculos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Veiculo veiculo = await veiculos.GetByIdAsync(id);
      if (veiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<VeiculoViewModel>(veiculo);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.Classe = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.Classe);
      }
      ViewBag.Categoria = new SelectList(Categoria.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.Categoria);

      return View(viewModel);
    }

    // POST: Veiculos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(VeiculoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<CVeiculo> cVeiculos = new Services<CVeiculo>()) {
        ViewBag.Classe = new SelectList(await cVeiculos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Classe }), "Id", "Name", viewModel.Classe);
      }
      ViewBag.Categoria = new SelectList(Categoria.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.Categoria);

      try {
        if (ModelState.IsValid) {
          Veiculo veiculo = mapper.Map<Veiculo>(viewModel);
          await veiculos.Update(veiculo);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Veiculos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Veiculo veiculo = await veiculos.GetFirstAsync(v => v.Id == id);
      if (veiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<VeiculoViewModel>(veiculo);
      return View(viewModel);
    }

    // POST: Veiculos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Veiculo veiculo = await veiculos.GetByIdAsync(id);
      if (veiculo != null) {
        await veiculos.Delete(veiculo);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (veiculos != null)) {
        veiculos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
