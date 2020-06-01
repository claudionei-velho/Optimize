using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dto.Extensions {
  public class ExpressionParameterReplacer : ExpressionVisitor {
    private readonly IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements;

    public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters) {
      ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
      for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++) { 
        ParameterReplacements.Add(fromParameters[i], toParameters[i]); 
      }
    }

    protected override Expression VisitParameter(ParameterExpression node) {
      if (ParameterReplacements.TryGetValue(node, out ParameterExpression replacement)) { 
        node = replacement; 
      }
      return base.VisitParameter(node);
    }
  }
}
