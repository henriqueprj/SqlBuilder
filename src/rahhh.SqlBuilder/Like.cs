using System;

namespace rahhh.SqlBuilder.Expressions
{
    public static class Like
    {
        public static string StartsWith(string value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : $"{EncodeSql(value)}%";

        public static string EndsWith(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : $"%{EncodeSql(value)}";
        }

        public static string Contains(string value)
        {
            
            return string.IsNullOrWhiteSpace(value) ? string.Empty : $"%{EncodeSql(value)}%";
        }

        private static string EncodeSql(string value) => value.Replace("[", "[[]").Replace("%", "[%]");

        public static string ApplyPattern(string value, LikePattern pattern)
        {
            switch (pattern)
            {
                case LikePattern.StartsWith:
                    return StartsWith(value);
                case LikePattern.EndsWith:
                    return EndsWith(value);
                case LikePattern.Contains:
                    return Contains(value);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}