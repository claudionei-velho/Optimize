using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Dto.Models;
using UI.Models;

namespace UI.Controllers {
  [Authorize]
  public class ISinoticosController : Controller {
    private readonly Services<ISinotico> iSinoticos = new Services<ISinotico>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ISinoticoViewModel, ISinotico>().ReverseMap();
                                          }).CreateMapper();

    // GET: ISinoticos
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<ISinoticoViewModel>>(await iSinoticos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (iSinoticos != null)) {
        iSinoticos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
