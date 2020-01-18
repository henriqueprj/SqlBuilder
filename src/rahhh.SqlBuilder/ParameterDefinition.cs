using System;
using System.Data;

namespace rahhh.SqlBuilder.Expressions
{
    public class ParameterDefinition
    {
        internal static readonly ParameterDefinition[] EmptyArray = Array.Empty<ParameterDefinition>();
        
        public ParameterDefinition(string name, object value)
            : this(name, value, null, null, null, null)
        {
            Name = name;
            Value = value;
        }

        public ParameterDefinition(string name, object value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null)
        {
            Name = name;
            Value = value;
            DbType = dbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public DbType? DbType { get; set; }
        public int? Size { get; set; }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
    }
}
