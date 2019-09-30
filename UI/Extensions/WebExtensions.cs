using System.Web.Mvc;

using Dto.Models;

namespace UI.Extensions {
  public static class WebExtensions {
    public static void AddModelException(this ModelStateDictionary modelState, OptimizerException ex) {
      foreach (var error in ex.Errors) {
        modelState.AddModelError(error.Field, error.Message);
      }
    }
  }
}
