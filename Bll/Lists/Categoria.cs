using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public class Categoria {
    public readonly Dictionary<int, string> Data;

    public Categoria() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Leve" },
        { 2, "Pesado" },
        { 3, "Trucado" },
        { 4, "Especial" }
      };
    }

    public IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
