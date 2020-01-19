using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.SqlGenerator
{
    public class SqlGenerator
    {
        public (string Sql, ParameterDefinition[] Parameters) Generate(Expression expression)
        {
            var parameters = new SqlParameterVisitor().ExtractParameters(expression);
            var sql = new SqlGeneratorVisitor().Evaluate(expression);

            return (sql, parameters);
        }
    }
}
