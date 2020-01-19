using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class AndExpression : BinaryExpression
    {
        public AndExpression(Expression left, Expression right) : base(left, right)
        {
        }

        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
        
        public override BinaryExpression Create(Expression left, Expression right) => new AndExpression(left, right);
    }

    public partial class BooleanExpression
    {
        public static BooleanExpression operator &(BooleanExpression left, BooleanExpression right) => And(left, right);
    }
}
