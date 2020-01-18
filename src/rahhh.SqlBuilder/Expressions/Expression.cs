using System;
using System.Linq;
using System.Text.RegularExpressions;
using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public abstract partial class Expression : IExpressionVisitable
    {
        public abstract void Accept(IExpressionVisitor visitor);
        
        public static AndExpression And(BooleanExpression left, BooleanExpression right)
        {           
            return new AndExpression(left, right);
        }

        public static AndExpression And(BooleanExpression left, BooleanExpression right, params BooleanExpression[] expressions)
        {           
            var expression = new AndExpression(left, right);
            if (expressions == null) 
                return expression;

            foreach (var other in expressions)
                expression = new AndExpression(expression, other);

            return expression;
        }
        
        public static OrExpression Or(BooleanExpression left, BooleanExpression right)
        {            
            return new OrExpression(left, right);
        }
        
        public static OrExpression Or(Expression left, Expression right, params Expression[] expressions)
        {
            var expression = new OrExpression(left, right);
            if (expressions == null) 
                return expression;
            
            foreach (var other in expressions)
                expression = new OrExpression(expression, other);

            return expression;
        }

        public static NotExpression Not(BooleanExpression expression) => new NotExpression(expression);
        public static bool operator true(Expression _) => false;
        public static bool operator false(Expression _) => false;
    }
}