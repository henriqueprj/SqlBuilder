namespace rahhh.SqlBuilder.Expressions
{
    public abstract class BinaryExpression : BooleanExpression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        protected BinaryExpression(Expression left, Expression right)
        {
            Guard.NotNull(left, nameof(left));
            Guard.NotNull(right, nameof(right));
            
            Left = left;
            Right = right;
        }

        public abstract BinaryExpression Create(Expression left, Expression right);
    }
}
