using System;
using System.Net.Http.Headers;
using rahhh.SqlBuilder.Expressions;
using Xunit;
using FluentAssertions;
using rahhh.SqlBuilder.SqlGenerator;
using Xunit.Sdk;

namespace rahhh.SqlBuilder.Tests
{
    public class ExpressionsTests
    {
        [Fact]
        public void Test1()
        {
            var a = new ConditionExpression("1 = 1");
            var b = new ConditionExpression("2 = 2");
            var c = new ConditionExpression("3 = 3");
            
            var exprA = a && !(b || c);

            var exprB = Expression.And(
                a,
                Expression.Not(
                    Expression.Or(
                        b,
                        c
                    )
                )
            );
            
            AssertExpression(exprA);
            
            AssertExpression(exprB);

            void AssertExpression(Expression expr)
            {
                expr.Should().NotBeNull();

                expr.Should().BeOfType<AndExpression>();

                var andExpr = expr.As<AndExpression>();
                
                andExpr.Left.Should().BeOfType<ConditionExpression>().Which.Predicate.Should().Be("1 = 1");
            
                andExpr.Right.Should().BeOfType<NotExpression>();

                var notExpr = andExpr.Right.As<NotExpression>();
            
                notExpr.Operand.Should().BeOfType<OrExpression>();
            
                notExpr.Operand.As<OrExpression>().Left.Should().BeOfType<ConditionExpression>().Which.Predicate
                    .Should().Be("2 = 2");
            
                notExpr.Operand.As<OrExpression>().Right.Should().BeOfType<ConditionExpression>().Which.Predicate
                    .Should().Be("3 = 3");    
            }
        }

        // [Fact]
        // public void TestarGeradorSql()
        // {
        //     var a = new ConditionExpression("A = 1");
        //     var b = new ConditionExpression("B = 2");
        //     var c = new ConditionExpression("C = 3");
        //     var d = new ConditionExpression("D = 4");
        //     var e = new ConditionExpression("E = 5");
            
        //     var expr = a || b || c && d && e;
            
        //     var visitor = new SqlExpressionVisitor();
        //     var (sql, parameters) = visitor.Build(expr);

        //     sql.Should().Be("A = 1 OR B = 2 OR C = 3 AND D = 4 AND E = 5");
        // }

        [Fact]
        public void AorB()
        {
            var a = new ConditionExpression("A = 1");
            var b = new ConditionExpression("B = 2");    
            
            var expr = a || b;
            
            var sql = Eval(expr);

            sql.Should().Be("A = 1 OR B = 2");
        }

        [Fact]
        public void A_and_B()
        {
            var a = new ConditionExpression("A = 1");
            var b = new ConditionExpression("B = 2");    
            
            var expr = a && b;
            
            var sql = Eval(expr);

            sql.Should().Be("A = 1 AND B = 2");
        }

        [Fact]
        public void A_and_B_and_C()
        {
            var a = new ConditionExpression("A = 1");
            var b = new ConditionExpression("B = 2");    
            var c = new ConditionExpression("C = 3");    
            
            var expr = a && b && c;
            
            var sql = Eval(expr);

            sql.Should().Be("A = 1 AND B = 2 AND C = 3");
        }

        [Fact]
        public void A_or_B_or_C()
        {
            var a = new ConditionExpression("A = 1");
            var b = new ConditionExpression("B = 2");    
            var c = new ConditionExpression("C = 3");    
            
            var expr = a || b || c;
            
            var sql = Eval(expr);

            sql.Should().Be("A = 1 OR B = 2 OR C = 3");
        }

        [Fact]
        public void A_or_B_and_C()
        {
            var a = new ConditionExpression("A");
            var b = new ConditionExpression("B");    
            var c = new ConditionExpression("C");    
            
            var expr = a || b && c;

            // var expr =
            //     Expression.Or(
            //         a,
            //         Expression.And(
            //             b,
            //             c
            //         )
            //     );
            
            var sql = Eval(expr);

            sql.Should().Be("A OR B AND C");
        }
        
        [Fact]
        public void LP_A_and_B_or_c_RP_and_d()
        {
            var a = new ConditionExpression("A");
            var b = new ConditionExpression("B");    
            var c = new ConditionExpression("C");
            var d = new ConditionExpression("D");
            
            var expr = (a && b || c) && d;

            var sql = Eval(expr);

            sql.Should().Be("(A AND B OR C) AND D");
        }
        
        [Fact]
        public void A_and_LP_B_or_c_and_d_RP()
        {
            var a = new ConditionExpression("A");
            var b = new ConditionExpression("B");    
            var c = new ConditionExpression("C");
            var d = new ConditionExpression("D");
            
            var expr = a && (b || c && d);

            var sql = Eval(expr);

            sql.Should().Be("A AND (B OR C AND D)");
        }

        [Fact]
        public void Should_not_render_false_condition()
        {
            var a = new ConditionExpression(false, "A");
            
            var sql = Eval(a);

            sql.Should().Be("");
        }

        private string Eval(Expression expr)
        {
            var visitor = new SqlExpressionVisitor();
            var (sql, _) = visitor.Build(expr);
            return sql;
        }
    }
}