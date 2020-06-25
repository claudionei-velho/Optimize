using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Direcao {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Manual" },
        { 2, "Hidráulica" },
        { 3, "Elétrica" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
