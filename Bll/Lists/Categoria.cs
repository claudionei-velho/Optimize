using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Categoria {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Leve" },
        { 2, "Pesado" },
        { 3, "Trucado" },
        { 4, "Especial" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
