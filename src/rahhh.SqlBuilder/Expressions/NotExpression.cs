using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class NotExpression : BooleanExpression
    {
        public BooleanExpression Operand { get; }
        
        public NotExpression(BooleanExpression expression)
        {
            Guard.NotNull(expression, nameof(expression));
            
            Operand = expression;
        }
        
        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
    }

    public partial class BooleanExpression
    {
        public static BooleanExpression operator !(BooleanExpression expression) => new NotExpression(expression);
    }
}