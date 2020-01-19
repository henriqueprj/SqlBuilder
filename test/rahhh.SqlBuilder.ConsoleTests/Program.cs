using System;
using Microsoft.FSharp.Collections;
using rahhh.FsSqlBuilder;
using rahhh.SqlBuilder.Expressions;
using rahhh.SqlBuilder.SqlGenerator;
using Expression = rahhh.SqlBuilder.Expressions.Expression;
using ParameterDefinition = rahhh.FsSqlBuilder.ParameterDefinition;

namespace rahhh.SqlBuilder.ConsoleTests
{

    
    class Program
    {
        static void Main(string[] args)
        {
            //FsSqlBuilder.Exp.And()


            
            // var asdf = new FsSqlBuilder.SqlConditionalExp(true, "A = 1", new ParameterDefinition("@A"));

            var builder = new ExpBuilder();
            builder.SqlCondition("A = @A", p => p.Varchar("@A", "Henrique", 50));
            
            
            var a = new ConditionExpression("A = 1");
            var b = new ConditionExpression("B = 2");
            var c = new ConditionExpression("C = 3");
            var d = new ConditionExpression("D = 4");

            var r = (Op(1, false) || Op(2)) && Op(3, false) || (Op(4) && Op(5, false));
            

            var exprA = a && (b && c) && d;
            var exprB = a && b && c && d;
            
            var exprC = a && (b || c) && d;
            var exprD = a && b || c && d;
            
            var exprE = a && (b || c && d);
            
            var exprF = a || (b || c && d);
            
            var exprG = a || b || c || d;
            
            var exprH = a || b && c;

            Console.WriteLine($"A) a && (b && c) && d |#####| {RenderSql(exprA)}");
            Console.WriteLine($"B) a && b && c && d   |#####| {RenderSql(exprB)}");
            Console.WriteLine($"C) a && (b || c) && d |#####| {RenderSql(exprC)}");
            Console.WriteLine($"D) a && b || c && d   |#####| {RenderSql(exprD)}");
            Console.WriteLine($"E) a && (b || c && d) |#####| {RenderSql(exprE)}");
            Console.WriteLine($"F) a || (b || c && d) |#####| {RenderSql(exprF)}");
            Console.WriteLine($"G) a || b || c || d   |#####| {RenderSql(exprG)}");
            Console.WriteLine($"H) a || b && c        |#####| {RenderSql(exprH)}");

            Console.WriteLine();
        }

        private static string RenderSql(Expression expression)
        {
            var visitor = new SqlGeneratorVisitor();
            var sql = visitor.Evaluate(expression);
            return sql;
        }
        
        private static bool Op(int i, bool r = true)
        {
            Console.WriteLine("OP-{0:00}", i);
            return r;
        }
    }
}
