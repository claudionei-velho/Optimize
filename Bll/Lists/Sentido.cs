using System.Collections.Generic;

namespace Bll.Lists {
  public class Sentido {
    public Dictionary<string, string> Data;

    public Sentido() {
      Data = new Dictionary<string, string> {
        { "AB", "A -> B" },
        { "BA", "B -> A" }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in Data) {
        result.Add(new { Id = item.Key, Name = item.Value });
      }
      return result;
    }
  }
}
