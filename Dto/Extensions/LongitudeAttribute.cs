using System.ComponentModel.DataAnnotations;

namespace Dto.Extensions {
  public sealed class LongitudeAttribute : ValidationAttribute {
    private readonly float MinValue;
    private readonly float MaxValue;

    private const string LongitudeError = "A Longitude deve estar entre -180 e 180 graus";

    public LongitudeAttribute(float minValue, float maxValue) {
      this.MinValue = minValue > -180f ? minValue : -180f;
      this.MaxValue = maxValue < 180f ? maxValue : 180f;
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
      return LongitudeError;
    }
  }
}
