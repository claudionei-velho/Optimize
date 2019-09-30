using System.Collections.Generic;

namespace Bll.Lists {
  public class Transmissao {
    public Dictionary<int, string> Data;

    public Transmissao() {
      Data = new Dictionary<int, string> {
        { 1, "Manual" },
        { 2, "Automática" }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in Data) {
        result.Add(new { Id = item.Key.ToString(), Name = item.Value });
      }
      return result;
    }
  }
}
