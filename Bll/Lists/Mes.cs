using System.Collections.Generic;

namespace Bll.Lists {
  public class Mes {
    public IDictionary<int, string> Data;
    public IDictionary<int, string> Short;
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

      Short = new Dictionary<int, string> {
        { 1, "Jan" },
        { 2, "Fev" },
        { 3, "Mar" },
        { 4, "Abr" },
        { 5, "Maio" },
        { 6, "Jun" },
        { 7, "Jul" },
        { 8, "Ago" },
        { 9, "Set" },
        { 10, "Out" },
        { 11, "Nov" },
        { 12, "Dez" }
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
