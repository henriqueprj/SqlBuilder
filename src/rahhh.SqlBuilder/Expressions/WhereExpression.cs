using System;
using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class WhereExpression : UnaryExpression
    {
        public WhereExpression(BooleanExpression expression) : base(expression)
        {
        }
        
        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);

        public override UnaryExpression Create(BooleanExpression expression) => new WhereExpression(expression);
    }
}
