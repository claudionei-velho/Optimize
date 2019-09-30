using System.Collections.Generic;

using Dto.Models;

namespace Bll {
  public class BaseValidator<T> { 
    private readonly List<ErrorField> errors = new List<ErrorField>();

    public void AddError(ErrorField errorField) {
      errors.Add(errorField);
    }

    public virtual void Validate(T item) {
      CheckErrors();
    }

    protected void CheckErrors() {
      if (errors.Count > 0) {
        throw new OptimizerException(errors);
      }
    }
  }
}
