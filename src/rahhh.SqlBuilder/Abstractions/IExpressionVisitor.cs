using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.Abstractions
{
    public interface IExpressionVisitor
    {
        void Visit(ConditionExpression expr);
        void Visit(AndExpression expr);
        void Visit(OrExpression expr);
        void Visit(SqlParameterExpression expr);
        void Visit(EmptyExpression expr);
        void Visit(NotExpression expr);
        void Visit(WhereExpression expr);
    }
}
