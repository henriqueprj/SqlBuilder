using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public sealed class NotExpression : UnaryExpression
    {
        public NotExpression(BooleanExpression expression) 
            : base(expression)
        {
        }
        
        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
        
        public override UnaryExpression Create(BooleanExpression expression) => new NotExpression(expression);
    }

    public partial class BooleanExpression
    {
        public static BooleanExpression operator !(BooleanExpression expression) => new NotExpression(expression);
    }
}
