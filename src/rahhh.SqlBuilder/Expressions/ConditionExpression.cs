using System;
using System.Linq;
using System.Text.RegularExpressions;
using rahhh.SqlBuilder.Abstractions;

namespace rahhh.SqlBuilder.Expressions
{
    public class ConditionExpression : BooleanExpression
    {       
        public string Predicate { get; }
        
        public ParameterDefinition[] Parameters { get; }

        public bool Condition { get; }

        public ConditionExpression(string predicate) 
            : this(true, predicate, ParameterDefinition.EmptyArray)
        {
        }
        
        public ConditionExpression(bool condition, string predicate)
            : this(condition, predicate, ParameterDefinition.EmptyArray)
        {
        }

        public ConditionExpression(string predicate, ParameterDefinition[] parameters) 
            : this(true, predicate, parameters)
        {
        }
        
        

        public ConditionExpression(bool condition, string predicate, ParameterDefinition[] parameters)
        {
            Condition = condition;
            Predicate = predicate;
            Parameters = parameters ?? ParameterDefinition.EmptyArray;
        }
        public override void Accept(IExpressionVisitor visitor) => visitor.Visit(this);
        
        
    }

    public abstract partial class Expression
    {
//        private static readonly Regex ParameterRegex = new Regex("\\@[@_a-zA-Z0-9]+", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
//
//        public static ConditionExpression Condition(string sqlExpression, params ParameterDefinition[] parameters)
//        {
//            SetParameterNameForAutoParameters(sqlExpression, parameters);
//            return new ConditionExpression(true, sqlExpression, parameters);
//        }
//
//        public static ConditionExpression Condition(bool condition, string sqlExpression, params ParameterDefinition[] parameters)
//        {
//            SetParameterNameForAutoParameters(sqlExpression, parameters);
//            return new ConditionExpression(condition, sqlExpression, parameters);
//        }
//        
//        public static ConditionExpression Condition(string sqlExpression, object value)
//        {
//            return Condition(sqlExpression, new ParameterDefinition(ParameterDefinitionBuilder.AutoParameterNameSymbol, value));
//        }
//        
//        public static ConditionExpression Condition(bool condition, string sqlExpression, object value)
//        {
//            return Condition(condition, sqlExpression, new ParameterDefinition(ParameterDefinitionBuilder.AutoParameterNameSymbol, value));
//        }
//        
//        public static ConditionExpression Condition(bool condition, string sqlExpression, Action<ParameterDefinitionBuilder> configureParameters)
//        {
//            if (string.IsNullOrEmpty(sqlExpression)) throw new ArgumentNullException(nameof(sqlExpression));
//            if (configureParameters == null) throw new ArgumentNullException(nameof(configureParameters));
//            
//            var paramBuilder = new ParameterDefinitionBuilder();
//
//            if (condition) configureParameters(paramBuilder);
//            
//            return Expression.Condition(condition, sqlExpression, paramBuilder.Parameters.ToArray());
//        }
//        
//
//        private static void CheckAutoParameter(ParameterDefinitionBuilder paramBuilder)
//        {
//            if (paramBuilder.Parameters.Any(p => p.Name == ParameterDefinitionBuilder.AutoParameterNameSymbol))
//                throw new ArgumentException("You must provide a parameter name for SqlParameterExpresion");
//        }
//        
//        private static void SetParameterNameForAutoParameters(string sqlExpression, ParameterDefinition[] parameterDefinitions)
//        {
//            foreach (var parameterDefinition in parameterDefinitions)
//            {
//                if (parameterDefinition.Name != ParameterDefinitionBuilder.AutoParameterNameSymbol)
//                    continue;
//
//                var parameterName = ExtractParameterName(sqlExpression);
//                if (parameterName == null)
//                    throw new InvalidOperationException("Nenhum parâmetro foi informado na expressão.");
//
//                parameterDefinition.Name = parameterName;
//            }
//        }              
//        
//        private static string ExtractParameterName(string expression)
//        {
//            var matches = ParameterRegex.Matches(expression);
//            if (matches.Count == 0)
//                return null;
//
//            if (matches.Cast<Match>().Select(x => x.Value).Distinct(StringComparer.InvariantCultureIgnoreCase).Count() > 1)
//                throw new Exception($"There are more than one parameter defined in the expression: '{expression}'");
//
//            return matches[0].Value;
//        }
    }
    
}
