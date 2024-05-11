using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;

namespace Core.Utils;

public static class ReflectionUtils
{
    /// <summary>
    /// Given a lambda expression that calls a method, returns the method info.
    /// </summary>
    public static MethodInfo GetMethodInfo(Expression<Action> expression)
        => GetMethodInfo((LambdaExpression)expression);

    /// <summary>
    /// Given a lambda expression that calls a method, returns the method info.
    /// </summary>
    public static MethodInfo GetMethodInfo(LambdaExpression expression)
    {
        if (expression.Body is not MethodCallExpression outermostExpression)
        {
            throw new ArgumentException("Invalid Expression. Expression should consists of a Method call only.");
        }

        return outermostExpression.Method;
    }

    /// <summary>
    /// Check if query is ordered with any of ordering methods of <see cref="Queryable"/>.
    /// </summary>
    public static bool IsQueryOrdered(IQueryable query)
    {
        Defend.Against.Null(query, nameof(query));

        var visitor = new OrderingTester();
        visitor.Visit(query.Expression);
        return visitor.orderingMethodFound;
    }

    private class OrderingTester : ExpressionVisitor
    {
        public bool orderingMethodFound;

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var name = node.Method.Name;

            if (node.Method.DeclaringType == typeof(Queryable) &&
                  (name.StartsWith(nameof(Queryable.OrderBy), StringComparison.Ordinal)
                || name.StartsWith(nameof(Queryable.OrderByDescending), StringComparison.Ordinal)
#if NET7_0_OR_GREATER
                || name.StartsWith(nameof(Queryable.Order), StringComparison.Ordinal)
                || name.StartsWith(nameof(Queryable.OrderDescending), StringComparison.Ordinal)
#endif
            ))
            {
                orderingMethodFound = true;
            }

            return base.VisitMethodCall(node);
        }
    }
}
