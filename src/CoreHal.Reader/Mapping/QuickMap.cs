using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoreHal.Reader.Mapping
{
    public static class QuickMap
    {
        public static KeyValuePair<string, string> Into<TEntity>(Expression<Func<TEntity, object>> mapInto, string mapFrom)
        {
            var propertyToMapInto = GetMemberName(mapInto.Body);

            return new KeyValuePair<string, string>(propertyToMapInto, mapFrom);
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("The expression cannot be null.");
            }

            if (expression is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression unaryExpression)
            {
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression.");
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }
}
