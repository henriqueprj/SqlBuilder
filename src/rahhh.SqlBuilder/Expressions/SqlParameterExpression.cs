using System;
using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    /// <summary>
    /// Expressão utilizada apenas para poder incluir um parâmetro (com valor) no momento da geração do SQL.
    /// </summary>
    public sealed class SqlParameterExpression : Expression
    {
        public bool Condition { get; }
        public ParameterDefinition[] Parameters { get; }

        public SqlParameterExpression(bool condition, ParameterDefinition parameter)
        {
            Condition = condition;
            Parameters = parameter == null ? Array.Empty<ParameterDefinition>() : new[] { parameter };
        }
        
        public SqlParameterExpression(bool condition, ParameterDefinition[] parameters)
        {                        
            Condition = condition;
            Parameters = parameters ?? ParameterDefinition.EmptyArray;
        }
        
        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public partial class Expression
    {
//        public static SqlParameterExpression Parameter(Action<ParameterDefinitionBuilder> configureParameters)
//        {
//            if (configureParameters == null) throw new ArgumentNullException(nameof(configureParameters));
//            
//            var paramBuilder = new ParameterDefinitionBuilder();
//            configureParameters(paramBuilder);
//            
//            CheckAutoParameter(paramBuilder);
//            
//            return new SqlParameterExpression(true, paramBuilder.Parameters.ToArray());
//        }
//        
//        public static SqlParameterExpression AddParameter(
//            bool condition,
//            Action<ParameterDefinitionBuilder> configureParameters)
//        {
//            if (configureParameters == null) throw new ArgumentNullException(nameof(configureParameters));
//            
//            var paramBuilder = new ParameterDefinitionBuilder();
//            if (condition) 
//                configureParameters(paramBuilder);
//            
//            CheckAutoParameter(paramBuilder);                        
//
//            return new SqlParameterExpression(condition, paramBuilder.Parameters.ToArray());;
//        }
    }
}