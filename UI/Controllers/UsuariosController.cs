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
  public class UsuariosController : Controller {
    private readonly Services<Usuario> usuarios = new Services<Usuario>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<UsuarioViewModel, Usuario>().ReverseMap();
                                          }).CreateMapper();

    // GET: Usuarios
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<UsuarioViewModel>>(await usuarios.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Usuarios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuario usuario = await usuarios.GetByIdAsync(id);
      if (usuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<UsuarioViewModel>(usuario);
      return View(viewModel);
    }

    // GET: Usuarios/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(UsuarioViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Usuario usuario = mapper.Map<Usuario>(viewModel);
          await usuarios.Insert(usuario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Usuarios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuario usuario = await usuarios.GetByIdAsync(id);
      if (usuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<UsuarioViewModel>(usuario);
      return View(viewModel);
    }

    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UsuarioViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Usuario usuario = mapper.Map<Usuario>(viewModel);
          await usuarios.Update(usuario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Usuarios/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Usuario usuario = await usuarios.GetByIdAsync(id);
      if (usuario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<UsuarioViewModel>(usuario);
      return View(viewModel);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Usuario usuario = await usuarios.GetByIdAsync(id);
      if (usuario != null) {
        await usuarios.Delete(usuario);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (usuarios != null)) {
        usuarios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
