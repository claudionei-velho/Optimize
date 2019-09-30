using System.Collections.Generic;

namespace Bll.Lists {
  public class Fluxo {
    public Dictionary<int, string> Data;

    public Fluxo() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Início" },
        { 2, "Passagem" },
        { 3, "Término" }        
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
