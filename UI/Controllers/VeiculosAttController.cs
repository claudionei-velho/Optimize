using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Services;
using Dto.Models;
using UI.Models;

namespace UI.Controllers {
  [Authorize]
  public class VeiculosAttController : Controller {
    private readonly VeiculoAttService veiculosAtt = new VeiculoAttService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<VeiculoAttViewModel, VeiculoAtt>().ReverseMap();
                                          }).CreateMapper();

    // GET: VeiculosAtt
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<VeiculoAttViewModel>>(await veiculosAtt.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: VeiculosAtt
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<VeiculoAttViewModel>>(
                          await veiculosAtt.GetAllAsync(q => q.Classe == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: VeiculosAtt/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      VeiculoAtt veiculoAtt = await veiculosAtt.GetFirstAsync(v => v.Id == id);
      if (veiculoAtt == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<VeiculoAttViewModel>(veiculoAtt);
      return View(viewModel);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (veiculosAtt != null)) {
        veiculosAtt.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
