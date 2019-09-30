using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class EUsuariosController : Controller {
    private readonly EUsuarioService eUsuarios = new EUsuarioService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<EUsuarioViewModel, EUsuario>().ReverseMap();
                                          }).CreateMapper();

    // GET: EUsuarios
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;

      var viewModel = mapper.Map<IEnumerable<EUsuarioViewModel>>(
                          await eUsuarios.GetAllAsync(orderBy: q => q.OrderBy(p => p.EmpresaId).ThenBy(p => p.Id)));
      if (user.ID > 1) {
        viewModel = mapper.Map<IEnumerable<EUsuarioViewModel>>(
                        await eUsuarios.GetAllAsync(q => q.UsuarioId == user.ID, 
                                                    q => q.OrderBy(p => p.EmpresaId).ThenBy(p => p.Id)));
      }
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: EUsuarios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EUsuario eUsuario = await eUsuarios.GetFirstAsync(e => e.Id == id);
      if (eUsuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EUsuarioViewModel>(eUsuario);
      return View(viewModel);
    }

    // GET: EUsuarios/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<Usuario> usuarios = new Services<Usuario>()) {
        ViewBag.UsuarioId = new SelectList(usuarios.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Nome }), "Id", "Name");
      }
      return View();
    }

    // POST: EUsuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(EUsuarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Usuario> usuarios = new Services<Usuario>()) {
        ViewBag.UsuarioId = new SelectList(await usuarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Nome }), "Id", "Name", viewModel.UsuarioId);
      }

      try {
        if (ModelState.IsValid) {
          EUsuario eUsuario = mapper.Map<EUsuario>(viewModel);
          await eUsuarios.Insert(eUsuario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: EUsuarios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EUsuario eUsuario = await eUsuarios.GetByIdAsync(id);
      if (eUsuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EUsuarioViewModel>(eUsuario);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Usuario> usuarios = new Services<Usuario>()) {
        ViewBag.UsuarioId = new SelectList(await usuarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Nome }), "Id", "Name", viewModel.UsuarioId);
      }
      return View(viewModel);
    }

    // POST: EUsuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(EUsuarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Usuario> usuarios = new Services<Usuario>()) {
        ViewBag.UsuarioId = new SelectList(await usuarios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Nome }), "Id", "Name", viewModel.UsuarioId);
      }

      try {
        if (ModelState.IsValid) {
          EUsuario eUsuario = mapper.Map<EUsuario>(viewModel);
          await eUsuarios.Update(eUsuario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Custos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      EUsuario eUsuario = await eUsuarios.GetFirstAsync(e => e.Id == id);
      if (eUsuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<EUsuarioViewModel>(eUsuario);
      return View(viewModel);
    }

    // POST: Custos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      EUsuario eUsuario = await eUsuarios.GetByIdAsync(id);
      if (eUsuario != null) {
        await eUsuarios.Delete(eUsuario);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (eUsuarios != null)) {
        eUsuarios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
