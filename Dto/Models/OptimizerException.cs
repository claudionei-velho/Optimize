using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Dto.Models {
  [Serializable]
  public class OptimizerException : Exception {
    public List<ErrorField> Errors { get; private set; }

    public string GetErrors() {
      StringBuilder sb = new StringBuilder();
      foreach (ErrorField item in Errors) {
        sb.AppendLine(string.Format("{0}: {1}", item.Field, item.Message));
      }
      sb.AppendLine();
      return sb.ToString();
    }

    public OptimizerException(List<ErrorField> errorFields) {
      Errors = errorFields;
    }

    public OptimizerException(ErrorField error) {
      Errors = new List<ErrorField> { error };
    }

    public OptimizerException() { }
    public OptimizerException(string message) : base(message) { }
    public OptimizerException(string message, Exception inner) : base(message, inner) { }

    protected OptimizerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
