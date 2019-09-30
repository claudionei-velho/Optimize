using System.Collections.Generic;

namespace Bll.Lists {
  public class Categoria {
    public Dictionary<int, string> Data;

    public Categoria() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Leve" },
        { 2, "Pesado" },
        { 3, "Trucado" },
        { 4, "Especial" }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in Data) {
        if (item.Key > 0) {
          result.Add(new { Id = item.Key.ToString(), Name = item.Value });
        }
      }
      return result;
    }
  }
}
