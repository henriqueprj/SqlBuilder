using System.Data;
using FluentAssertions;
using rahhh.SqlBuilder.Expressions;
using rahhh.SqlBuilder.SqlGenerator;
using Xunit;
using Xunit.Abstractions;

namespace rahhh.SqlBuilder.Tests
{
    public class SqlGeneratorVisitorTest
    {
        private readonly ITestOutputHelper _output;

        public SqlGeneratorVisitorTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Should_write_ConditionExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var expr = new ConditionExpression(true, "A = @A", new[] { parameter });
        
            var (sql, parameters) = Eval(expr);
        
            sql.Should().Be("A = @A");
            parameters.Should().Contain(parameter);
        }
        
        [Fact]
        public void Should_NOT_write_ConditionExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var expr = new ConditionExpression(false, "A = @A", new[] { parameter });
        
            var (sql, parameters) = Eval(expr);
        
            sql.Should().BeEmpty();
            parameters.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_do_nothing_with_an_empty_expression()
        {
            var expr = EmptyExpression.Empty;

            var (sql, parameters) = Eval(expr);

            sql.Should().BeEmpty();
            parameters.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_collect_SqlParameterExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var expr = new SqlParameterExpression(true, parameter);
        
            var (sql, parameters) = Eval(expr);

            sql.Should().BeEmpty();
            parameters.Should().Contain(parameter);
        }
        
        [Fact]
        public void Should_NOT_collect_SqlParameterExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var expr = new SqlParameterExpression(false, parameter);
        
            var (sql, parameters) = Eval(expr);

            sql.Should().BeEmpty();
            parameters.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_write_WhereExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var conditionExpr = new ConditionExpression(true, "A = @A", new[] { parameter });
            var whereExpr = new WhereExpression(conditionExpr);
        
            var (sql, parameters) = Eval(whereExpr);

            sql.Should().Be("WHERE " + conditionExpr.Predicate);
            parameters.Should().Contain(parameter);
        }
        
        [Fact]
        public void Should_not_write_WhereExpression_with_a_not_renderable_inner_expression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var conditionExpr = new ConditionExpression(false, "A = @A", new[] { parameter });    
            var whereExpr = new WhereExpression(conditionExpr);
        
            var (sql, parameters) = Eval(whereExpr);

            sql.Should().BeEmpty();
            parameters.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_write_NotExpression()
        {
            var parameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var conditionExpr = new ConditionExpression(true, "A = @A", new[] { parameter });    
            var notExpr = new NotExpression(conditionExpr);
        
            var (sql, parameters) = Eval(notExpr);

            sql.Should().Be("NOT " + conditionExpr.Predicate);
            parameters.Should().Contain(parameter);
        }

        [Fact]
        public void Should_enclose_inner_binary_expression_in_parens_for_NotExpression()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(true, "B = BA", new[] { bParameter });
            var expr = aExpr && bExpr;
            var notExpr = new NotExpression(expr);
        
            var (sql, parameters) = Eval(notExpr);
        
            sql.Should().Be("NOT (" + aExpr.Predicate + " AND " + bExpr.Predicate + ")");
            parameters.Should().Contain(new [] { aParameter, bParameter});
        }
        
        [Fact]
        public void Should_write_AndExpression()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(true, "B = @B", new[] { bParameter });
            var andExpr = aExpr && bExpr;
        
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be(aExpr.Predicate + " AND " + bExpr.Predicate);
            parameters.Should().Contain(new[] { aParameter, bParameter });
        }
        
        [Fact]
        public void Should_write_OrExpression()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(true, "B = @B", new[] { bParameter });
            var andExpr = aExpr || bExpr;
        
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be(aExpr.Predicate + " OR " + bExpr.Predicate);
            parameters.Should().Contain(new[] { aParameter, bParameter });
        }
        
        [Fact]
        public void Should_write_left_operand_only_of_OrExpression()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(false, "B = @B", new[] { bParameter });
            var andExpr = aExpr || bExpr;
        
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be(aExpr.Predicate);
            parameters.Should().Contain(new[] { aParameter });
        }
        
        [Fact]
        public void Should_enclose_left_right_operand_in_parens()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var cParameter = new ParameterDefinition("@C", 3, DbType.Int32);
            var dParameter = new ParameterDefinition("@D", 4, DbType.Int32);
            
            
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(true, "B = @B", new[] { bParameter });
            var cExpr = new ConditionExpression(true, "C = @C", new[] { cParameter });
            var dExpr = new ConditionExpression(true, "D = @D", new[] { dParameter });
            
            var andExpr = (aExpr || bExpr) && (cExpr || dExpr);
        
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be("(" + aExpr.Predicate + " OR " + bExpr.Predicate + ") AND (" + cExpr.Predicate + " OR " + dExpr.Predicate + ")");
            parameters.Should().Contain(new[] { aParameter, bParameter, cParameter, dParameter });
        }
        
        [Fact]
        public void Should_NOT_write_an_entire_binary_expression()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var cParameter = new ParameterDefinition("@C", 3, DbType.Int32);
            var dParameter = new ParameterDefinition("@D", 4, DbType.Int32);
            
            var aExpr = new ConditionExpression(false, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(false, "B = @B", new[] { bParameter });
            var cExpr = new ConditionExpression(true, "C = @C", new[] { cParameter });
            var dExpr = new ConditionExpression(true, "D = @D", new[] { dParameter });
            
            var andExpr = (aExpr || bExpr) && (cExpr || dExpr);
        
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be(cExpr.Predicate + " OR " + dExpr.Predicate);
            parameters.Should().Contain(new[] { cParameter, dParameter });
            
            _output.WriteLine(sql);
        }
        
        [Fact]
        public void Should_NOT_enclose_in_parens()
        {
            var aParameter = new ParameterDefinition("@A", 1, DbType.Int32);
            var bParameter = new ParameterDefinition("@B", 2, DbType.Int32);
            var cParameter = new ParameterDefinition("@C", 3, DbType.Int32);
            var dParameter = new ParameterDefinition("@D", 4, DbType.Int32);
            
            
            var aExpr = new ConditionExpression(true, "A = @A", new[] { aParameter });
            var bExpr = new ConditionExpression(true, "B = @B", new[] { bParameter });
            var cExpr = new ConditionExpression(true, "C = @C", new[] { cParameter });
            var dExpr = new ConditionExpression(true, "D = @D", new[] { dParameter });
            
            var andExpr = aExpr || bExpr && cExpr || dExpr;
            
            var (sql, parameters) = Eval(andExpr);

            sql.Should().Be(aExpr.Predicate + " OR " + bExpr.Predicate + " AND " + cExpr.Predicate + " OR " + dExpr.Predicate);
            parameters.Should().Contain(new[] { aParameter, bParameter, cParameter, dParameter });
        }

        private (string Sql, ParameterDefinition[] Parameters) Eval(Expression expression)
        {
            var generator = new SqlGenerator.SqlGenerator();
            return generator.Generate(expression);
        }
    }
}
