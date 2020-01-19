using System.Collections.Generic;
using System.Text;
using rahhh.SqlBuilder.Expressions;

namespace rahhh.SqlBuilder.SqlGenerator
{
    internal class SqlWriter
    {
        private readonly StringBuilder _buffer = new StringBuilder();

        public void AppendSpace()
        {
            _buffer.Append(" ");
        }

        public void Append(string sql)
        {
            if (!string.IsNullOrWhiteSpace(sql))
                _buffer.Append(sql);
        }

        public void OpenParenIf(bool condition)
        {
            if (!condition) return;
            _buffer.Append("(");
        }

        public void CloseParenIf(bool condition)
        {
            if (!condition) return;
            _buffer.Append(")");
        }

        public string Build()
        {
            return _buffer.ToString();
        }

        public void Reset()
        {
            _buffer.Clear();
        }
    }
}
