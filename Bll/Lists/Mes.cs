using System.Collections.Generic;

namespace Bll.Lists {
  public class Mes {
    public Dictionary<int, string> Data;

    public Mes() {
      Data = new Dictionary<int, string> {
        { 1, "Janeiro" },
        { 2, "Fevereiro" },
        { 3, "Março" },
        { 4, "Abril" },
        { 5, "Maio" },
        { 6, "Junho" },
        { 7, "Julho" },
        { 8, "Agosto" },
        { 9, "Setembro" },
        { 10, "Outubro" },
        { 11, "Novembro" },
        { 12, "Dezembro" }
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
