namespace rahhh.SqlBuilder.Expressions
{
    public abstract class UnaryExpression : BooleanExpression
    {
        public BooleanExpression Operand { get; }
        
        protected UnaryExpression(BooleanExpression expression)
        {
            Guard.NotNull(expression, nameof(expression));
            Operand = expression;
        }

        public abstract UnaryExpression Create(BooleanExpression expression);
    }
}
