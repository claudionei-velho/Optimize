using System;
using System.IO;
using System.Web.Mvc;

using MigraDoc.Rendering;

using UI.Reports.Docs;

namespace UI.Controllers {
  public class CustosCoController : Controller {
    public ActionResult Index(int id, int yr, int mh) {
      PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer {
          Document = new DROReport(id, yr, mh).CreateDocument()
      };

      string fileName = $"{Path.GetTempPath()}{Guid.NewGuid()}.pdf";
      pdfRenderer.RenderDocument();
      pdfRenderer.PdfDocument.Save(fileName);

      return File(fileName, "application/pdf");
    }
  }
}
