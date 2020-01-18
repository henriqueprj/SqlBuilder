using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class EmptyExpression : Expression
    {
        private EmptyExpression()
        {    
        }
        
        public static readonly EmptyExpression Empty = new EmptyExpression();
        
        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}