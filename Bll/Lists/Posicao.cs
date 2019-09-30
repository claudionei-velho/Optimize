using System.Collections.Generic;

namespace Bll.Lists {
  public class Posicao {
    public Dictionary<int, string> Data;

    public Posicao() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Dianteiro" },
        { 2, "Central" },
        { 3, "Traseiro" }
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
