using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class OrExpression : BinaryExpression
    {
        public OrExpression(Expression left, Expression right) : base(left, right)
        {
        }

        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
    }

    public partial class BooleanExpression
    {
        public static BooleanExpression operator |(BooleanExpression left, BooleanExpression right) => new OrExpression(left, right);
    }
}
