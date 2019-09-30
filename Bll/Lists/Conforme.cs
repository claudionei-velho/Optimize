using System.Collections.Generic;

namespace Bll.Lists {
  public class Conforme {
    public Dictionary<int, string> Data;

    public Conforme() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Disponível" },
        { 2, "Indisponível" },
        { 3, "Terceirizado" }
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
