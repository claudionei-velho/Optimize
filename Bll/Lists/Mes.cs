using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public class Mes {
    public readonly IDictionary<int, string> Data;
    public readonly IDictionary<int, string> Short;
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

    public IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.ToList();
    }
  }
}
