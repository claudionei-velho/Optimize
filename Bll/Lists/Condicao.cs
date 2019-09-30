using System.Collections.Generic;

namespace Bll.Lists {
  public class Condicao {
    public Dictionary<int, string> Data;

    public Condicao() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Excelente" },
        { 2, "Ótimo(a)" },
        { 3, "Bom" },
        { 4, "Regular" },
        { 5, "Ruim" },
        { 6, "Péssimo(a)" }
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
