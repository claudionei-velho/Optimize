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
  public class HorariosController : Controller {
    private HorarioService horarios = new HorarioService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<HorarioViewModel, Horario>().ReverseMap();
                                          }).CreateMapper();

    // GET: Horarios
    public async Task<ActionResult> Index(string currentFilter, string search, int? diaId, int? periodoId, int? page) {
      ViewBag.CurrentFilter = currentFilter;
      ViewBag.SearchString = search;
      ViewBag.DiaId = diaId;
      ViewBag.PeriodoId = periodoId;

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.horarios = new HorarioService(user.ID);

      IEnumerable<Horario> source = await horarios.GetAllAsync();
      if (!string.IsNullOrWhiteSpace(search)) {
        switch (int.Parse(currentFilter)) {
          case 1:
            source = await horarios.GetAllAsync(q => q.Linha.Denominacao.Contains(search));
            break;
          case 2:
            source = await horarios.GetAllAsync(q => q.Linha.Denominacao.Contains(search) && (q.DiaId == diaId));
            break;
          default:
            using (Services<PrLinha> prLinhas = new Services<PrLinha>()) {
              int periodo = prLinhas.GetFirst(p => p.Linha.Denominacao.Contains(search) && (p.PeriodoId == periodoId)).Id;
              source = await horarios.GetAllAsync(q => q.Linha.Denominacao.Contains(search) &&
                                                       (q.DiaId == diaId) && (q.PeriodoId == periodo));
            }
            break;
        }
      }
      var viewModel = mapper.Map<IEnumerable<HorarioViewModel>>(source);
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Horarios
    public async Task<ActionResult> Filter(int? id, string currentFilter, int? diaId, int? periodoId, int page = 1) {
      ViewBag.CurrentFilter = currentFilter;
      ViewBag.DiaId = diaId;
      ViewBag.PeriodoId = periodoId;

      IEnumerable<Horario> source = await horarios.GetAllAsync(q => q.LinhaId == id);
      if (!string.IsNullOrWhiteSpace(currentFilter)) {
        switch (int.Parse(currentFilter)) {
          case 1:
            source = await horarios.GetAllAsync(q => q.LinhaId == id && (q.DiaId == diaId));
            break;
          case 2:
            using (Services<PrLinha> prLinhas = new Services<PrLinha>()) {
              int periodo = prLinhas.GetFirst(p => p.LinhaId == id && (p.PeriodoId == periodoId)).Id;
              source = await horarios.GetAllAsync(q => (q.LinhaId == id) && (q.DiaId == diaId) &&
                                                       (q.PeriodoId == periodo));
            }
            break;
          default:
            source = await horarios.GetAllAsync(q => q.LinhaId == id);
            break;
        }
      }
      var viewModel = mapper.Map<IEnumerable<HorarioViewModel>>(source);
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: Horarios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Horario horario = await horarios.GetFirstAsync(h => h.Id == id);
      if (horario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<HorarioViewModel>(horario);
      return View(viewModel);
    }

    // GET: Horarios/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new { 
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name");
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name");
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      return View();
    }

    // POST: Horarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(HorarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }

      try {
        if (ModelState.IsValid) {
          Horario horario = mapper.Map<Horario>(viewModel);
          await horarios.Insert(horario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Horarios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Horario horario = await horarios.GetByIdAsync(id);
      if (horario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<HorarioViewModel>(horario);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            },
            q => q.LinhaId == viewModel.LinhaId), "Id", "Name", viewModel.AtendimentoId);
      }
      return View(viewModel);
    }

    // POST: Horarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(HorarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.DiaId = new SelectList(Workday.GetAll(), "Id", "Name", viewModel.DiaId);
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            },
            q => q.LinhaId == viewModel.LinhaId), "Id", "Name", viewModel.AtendimentoId);
      }

      try {
        if (ModelState.IsValid) {
          Horario horario = mapper.Map<Horario>(viewModel);
          await horarios.Update(horario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Horarios/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Horario horario = await horarios.GetFirstAsync(h => h.Id == id);
      if (horario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<HorarioViewModel>(horario);
      return View(viewModel);
    }

    // POST: Horarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Horario horario = await horarios.GetByIdAsync(id);
      if (horario != null) {
        await horarios.Delete(horario);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (horarios != null)) {
        horarios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
