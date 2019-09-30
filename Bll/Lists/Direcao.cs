using System.Collections.Generic;

namespace Bll.Lists {
  public class Direcao {
    public Dictionary<int, string> Data;

    public Direcao() {
      this.Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Manual" },
        { 2, "Hidráulica" },
        { 3, "Elétrica" }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in this.Data) {
        if (item.Key > 0) {
          result.Add(new { Id = item.Key.ToString(), Name = item.Value });
        }
      }
      return result;
    }
  }
}
