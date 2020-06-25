using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Fluxo {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Início" },
        { 2, "Passagem" },
        { 3, "Término" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
