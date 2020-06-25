using System.Collections.Generic;

namespace Dto.Lists {
  public static class Fluxo {
    public static IDictionary<int, string> Items = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Início" },
        { 2, "Passagem" },
        { 3, "Término" }
    };
  }
}
