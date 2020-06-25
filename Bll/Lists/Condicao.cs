using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Condicao {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Excelente" },
        { 2, "Ótimo(a)" },
        { 3, "Bom" },
        { 4, "Regular" },
        { 5, "Ruim" },
        { 6, "Péssimo(a)" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
