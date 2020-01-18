using System;
using System.Collections.Generic;
using System.Text;
using rahhh.SqlBuilder.Abstractions;
using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.SqlGenerator
{
    public class SqlExpressionVisitor : IExpressionVisitor
    {
        private readonly StringBuilder _sql = new StringBuilder();
        private readonly List<ParameterDefinition> _parameters = new List<ParameterDefinition>();
        
        private const string AndOperator = " AND ";
        private const string OrOperator = " OR ";

        public override string ToString() => _sql.ToString();

        public (string sql, IEnumerable<ParameterDefinition> parameters) Build(Expression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            
            _sql.Clear();
            _parameters.Clear();
            
            CollectParameters(expr);

            var renderableExpr = ExtractRenderableTree(expr);
            
            renderableExpr?.Accept(this);

            return (_sql.ToString(), _parameters.AsReadOnly());
        }

        private void Render(Expression expr)
        {
            
        }

        private void CollectParameters(Expression expr)
        {
            switch (expr)
            {
                case BinaryExpression binaryExpr:
                    CollectParameters(binaryExpr.Left);
                    CollectParameters(binaryExpr.Right);
                    break;
                case ConditionExpression conditionExpr:
                    if (!conditionExpr.Condition) return;
                    _parameters.AddRange(conditionExpr.Parameters);
                    break;
                case SqlParameterExpression sqlParamExpr:
                    if (!sqlParamExpr.Condition) return;
                    _parameters.AddRange(sqlParamExpr.Parameters);
                    break;
            }
        }
        
        void IExpressionVisitor.Visit(ConditionExpression expr)
        {
            if (!expr.Condition) return;
            _sql.Append(expr.Predicate);
        }

        void IExpressionVisitor.Visit(AndExpression expr) => RenderLogicalBinaryOperator(expr);

        void IExpressionVisitor.Visit(OrExpression expr) => RenderLogicalBinaryOperator(expr);
        
        void IExpressionVisitor.Visit(SqlParameterExpression expr)
        {
        }
        
        void IExpressionVisitor.Visit(EmptyExpression expr)
        {
        }
        
        void IExpressionVisitor.Visit(NotExpression expr)
        {
            if (!IsRenderable(expr.Operand)) 
                return;

            var encloseInParentheses = !(expr.Operand is BinaryExpression); 
            
            _sql.Append("!");

            OpenParenIf(encloseInParentheses);
            expr.Operand.Accept(this);
            CloseParenIf(encloseInParentheses);
        }
        
        private void RenderLogicalBinaryOperator(BinaryExpression expr)
        {
            var isLeftValid = IsRenderable(expr.Left);
            var isRightValid = IsRenderable(expr.Right);

            if (isLeftValid && isRightValid)
            {
                if (expr.Left is SqlParameterExpression || expr.Right is SqlParameterExpression)
                {
                    expr.Left.Accept(this);
                    expr.Right.Accept(this);
                } 
                else
                {
                    var op = GetOperatorToken(expr);
                    var actualOpPrecedence = GetOperatorPrecedence(expr);

                    var encloseInParenthesis = expr.Left is BinaryExpression l
                                               && actualOpPrecedence > GetOperatorPrecedence(l);
                    
                    
                    OpenParenIf(encloseInParenthesis);
                    expr.Left.Accept(this);
                    CloseParenIf(encloseInParenthesis);
                    
                    _sql.Append(op);

                    encloseInParenthesis = expr.Right is BinaryExpression r
                                               && actualOpPrecedence > GetOperatorPrecedence(r);
                    
                    OpenParenIf(encloseInParenthesis);
                    expr.Right.Accept(this);
                    CloseParenIf(encloseInParenthesis);
                }
            }
            else if (isLeftValid)
            {
                expr.Left.Accept(this);
            }
            else
            {
                expr.Right.Accept(this);
            }
        }

        private int GetOperatorPrecedence(BooleanExpression expression) =>
            expression switch
            {
                NotExpression _ => 3,
                AndExpression _ => 2,
                OrExpression _ => 1,
                _ => throw new NotSupportedException()
            };
        

        private void OpenParen() => _sql.Append("(");

        private void OpenParenIf(bool condition)
        {
            if (condition)
                _sql.Append("(");
        }

        private void CloseParen() => _sql.Append(")");
        
        private void CloseParenIf(bool condition)
        {
            if (condition)
                _sql.Append(")");
        }

        private Expression? ExtractRenderableTree(Expression expr)
        {
            static BinaryExpression CreateBinaryExpressionFromSource(BinaryExpression source, Expression left, Expression right) =>
                source switch
                {
                    AndExpression _ => new AndExpression(left, right),
                    OrExpression _  => new OrExpression(left, right),
                    _ => throw new InvalidOperationException("Invalid binary expression")
                };
            
            
            switch (expr)
            {
                case BinaryExpression binaryExpr:
                
                    var left = ExtractRenderableTree(binaryExpr.Left);
                    var right = ExtractRenderableTree(binaryExpr.Right);

                    if (left != null && right != null)
                    {
                        if (ReferenceEquals(left, binaryExpr.Left) && ReferenceEquals(right, binaryExpr.Right))
                        {
                            return expr;
                        }

                        return CreateBinaryExpressionFromSource(binaryExpr, left, right);
                    }
                    
                    return left ?? right;
                
                case ConditionExpression conditionExpr when IsRenderable(conditionExpr): return conditionExpr;
                case NotExpression notExpr when IsRenderable(notExpr): return notExpr;
                case SqlParameterExpression _: return null;
                default: return null;
            };


        }

        

        private static string GetOperatorToken(BinaryExpression binaryExpression)
        {
            return binaryExpression switch
            {
                AndExpression _ => AndOperator,
                OrExpression _ => OrOperator,
                _ => throw new ArgumentException($"Unsupported expression type '{binaryExpression.GetType().FullName}'")
            };
        }


        private static bool IsRenderable(Expression expression)
        {
            return expression switch
            {
                NotExpression notExpr => IsRenderable(notExpr.Operand),
                AndExpression andExpr => IsRenderable(andExpr.Left) && IsRenderable(andExpr.Right),
                OrExpression orExpr => IsRenderable(orExpr.Left) || IsRenderable(orExpr.Right),
                ConditionExpression expr => expr.Condition,
                SqlParameterExpression paramExpr => paramExpr.Condition,
                EmptyExpression _ => false,
                _ => false
            };
        }
    }
}