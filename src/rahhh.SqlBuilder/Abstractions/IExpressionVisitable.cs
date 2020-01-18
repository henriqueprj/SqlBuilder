namespace rahhh.SqlBuilder.Abstractions
{
    public interface IExpressionVisitable
    {
        void Accept(IExpressionVisitor visitor);
    }
}