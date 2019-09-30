namespace Dto.Models {
  public class ErrorField {
    public ErrorField(string field, string message) {
      this.Field = field;
      this.Message = message;
    }

    public string Field { get; private set; }
    public string Message { get; private set; }
  }
}
