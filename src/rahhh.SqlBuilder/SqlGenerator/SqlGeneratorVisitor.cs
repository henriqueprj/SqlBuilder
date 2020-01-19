using System;
using rahhh.SqlBuilder.Abstractions;
using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.SqlGenerator
{
    public class SqlGeneratorVisitor : IExpressionVisitor
    {
        enum LogicalOperator
        {
            NOT = 0,
            AND = -1,
            OR = -2
        }

        private readonly SqlWriter _writer = new SqlWriter();

        public string Evaluate(Expression expression)
        {
            _writer.Reset();

            var renderableExpression = GetRenderableExpression(expression);
            renderableExpression.Accept(this);
            
            return _writer.Build();
        }

        void IExpressionVisitor.Visit(ConditionExpression expr)
        {
            if (!expr.Condition) return;

            _writer.Append(expr.Predicate);
        }

        void IExpressionVisitor.Visit(AndExpression expr)
        {
            Visit(expr, LogicalOperator.AND);
        }

        void IExpressionVisitor.Visit(OrExpression expr)
        {
            Visit(expr, LogicalOperator.OR);
        }

        void IExpressionVisitor.Visit(SqlParameterExpression expr)
        {
        }

        void IExpressionVisitor.Visit(EmptyExpression expr)
        {
        }

        void IExpressionVisitor.Visit(NotExpression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var renderableExpression = GetRenderableExpression(expr);
            if (!(renderableExpression is NotExpression))
                return;

            _writer.Append("NOT ");

            var encloseInParens = expr.Operand is BinaryExpression;

            _writer.OpenParenIf(encloseInParens);
            expr.Operand.Accept(this);
            _writer.CloseParenIf(encloseInParens);
        }

        void IExpressionVisitor.Visit(WhereExpression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            
            var expression = GetRenderableExpression(expr);
            if (expression is EmptyExpression)
                return;
                    
            _writer.Append("WHERE ");

            expr.Operand.Accept(this);
        }

        private void Visit(BinaryExpression expr, LogicalOperator op)
        {
            bool encloseInParens;
            
            encloseInParens = expr.Left is BinaryExpression l && op > GetOperator(l);
            
            _writer.OpenParenIf(encloseInParens);
            expr.Left.Accept(this);
            _writer.CloseParenIf(encloseInParens);
            
            _writer.AppendSpace();
            _writer.Append(op.ToString());
            _writer.AppendSpace();

            encloseInParens = expr.Right is BinaryExpression r && op > GetOperator(r);

            _writer.OpenParenIf(encloseInParens);
            expr.Right.Accept(this);
            _writer.CloseParenIf(encloseInParens);
        }

        private LogicalOperator GetOperator(Expression expression) =>
            expression switch
            {
                AndExpression _ => LogicalOperator.AND,
                OrExpression _ => LogicalOperator.OR,
                NotExpression _ => LogicalOperator.NOT,
                _ => throw new InvalidOperationException()
            };

        private Expression GetRenderableExpression(Expression expression)
        {
            return expression switch
            {
                ConditionExpression expr => expr.Condition ? (Expression)expr : EmptyExpression.Empty,
                BinaryExpression expr => GetRenderableExpression(expr),
                UnaryExpression expr => GetRenderableExpression(expr),
                _ => EmptyExpression.Empty
            };
        }

        private Expression GetRenderableExpression(BinaryExpression expression)
        {
            var leftExpr = GetRenderableExpression(expression.Left);
            var rightExpr = GetRenderableExpression(expression.Right);

            if (ReferenceEquals(expression.Left, leftExpr) && ReferenceEquals(expression.Right, rightExpr))
                return expression;

            if (!(leftExpr is EmptyExpression || rightExpr is EmptyExpression))
                return expression.Create(leftExpr, rightExpr);

            if (leftExpr is EmptyExpression && rightExpr is EmptyExpression)
                return EmptyExpression.Empty;

            if (rightExpr is EmptyExpression)
                return leftExpr;

            return rightExpr;
        }
        
        private Expression GetRenderableExpression(UnaryExpression expression)
        {
            var expr = GetRenderableExpression(expression.Operand);
            if (expr is EmptyExpression)
                return expr;

            if (ReferenceEquals(expression.Operand, expr))
                return expression;
            
            // rebuild NotExpression with a renderable one
            if (expr is BooleanExpression b)
                return expression.Create(b);
            
            return EmptyExpression.Empty;
        }
    }
}
