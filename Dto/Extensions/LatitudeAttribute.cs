using System.ComponentModel.DataAnnotations;

namespace Dto.Extensions {
  public sealed class LatitudeAttribute : ValidationAttribute {
    private readonly float MinValue;
    private readonly float MaxValue;

    private const string LatitudeError = "A Latitude deve estar entre -90 e 90 graus";

    public LatitudeAttribute(float minValue, float maxValue) {
      this.MinValue = minValue > -90f ? minValue : -90f;
      this.MaxValue = maxValue < 90f ? maxValue : 90f;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
      if (value == null) {
        return ValidationResult.Success;
      }

      if ((float)value < this.MinValue || (float)value > this.MaxValue) {
        return new ValidationResult(GetMessage());
      }
      return ValidationResult.Success;
    }

    public static string GetMessage() {
      return LatitudeError;
    }
  }
}
