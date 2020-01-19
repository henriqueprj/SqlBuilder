using System.Collections.Generic;
using rahhh.SqlBuilder.Abstractions;
using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.SqlGenerator
{
    public class SqlParameterVisitor : IExpressionVisitor
    {
        private readonly List<ParameterDefinition> _parameters = new List<ParameterDefinition>();
        
        public ParameterDefinition[] ExtractParameters(Expression expression)
        {
            _parameters.Clear();
            expression.Accept(this);
            return _parameters.ToArray();
        }
        
        void IExpressionVisitor.Visit(ConditionExpression expr)
        {
            if (!expr.Condition) return;
            _parameters.AddRange(expr.Parameters);
        }

        void IExpressionVisitor.Visit(AndExpression expr)
        {
            expr.Left.Accept(this);
            expr.Right.Accept(this);
        }

        void IExpressionVisitor.Visit(OrExpression expr)
        {
            expr.Left.Accept(this);
            expr.Right.Accept(this);
        }

        void IExpressionVisitor.Visit(SqlParameterExpression expr)
        {
            if (!expr.Condition) return;
            _parameters.AddRange(expr.Parameters);
        }

        void IExpressionVisitor.Visit(EmptyExpression expr)
        {
        }

        void IExpressionVisitor.Visit(NotExpression expr)
        {
            expr.Operand.Accept(this);
        }

        void IExpressionVisitor.Visit(WhereExpression expr)
        {
            expr.Operand.Accept(this);
        }
    }
}
