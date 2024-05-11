using System.Linq.Expressions;
using Ardalis.GuardClauses;

namespace Core.Utils;

public class ExpressionUtils
{
    /// <summary>
    /// Returns expression (for IQueriable for example) that allows to get member you need or default value if any of submember on the path will be null.
    /// It's like: () => User?.Address?.City ?? "DefaultCity".
    /// </summary>
    /// <param name="memberAccessExpression">Lambda like: () => User.Address.City</param>
    /// <param name="defaultValue">Default value like: "DefaultCity"</param>
    /// <param name="newParameterExpression">Expression to be used as a root for the member accessors chain.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static Expression<Func<TIn, TOut>> GetNestedMemberOrDefaultLambda<TIn, TOut>
            (Expression<Func<TIn, TOut>> memberAccessExpression,
            TOut defaultValue,
            ParameterExpression newParameterExpression = default!)
    {
        Defend.Against.Null(memberAccessExpression, nameof(memberAccessExpression));
        Defend.Against.InvalidMemberAccessChain(memberAccessExpression, nameof(memberAccessExpression));

        var (expression, rootExpression) = GetNestedMemberOrDefaultExpression(memberAccessExpression, defaultValue, newParameterExpression);
        return Expression.Lambda<Func<TIn, TOut>>(expression, rootExpression);
    }

    /// <summary>
    /// Revert a boolean func expression body.
    /// <br></br>
    /// Example: from (x => x.IsArchive) build (x => !x.IsArchive).
    /// Can be compiled to SQL.
    /// </summary>
    public static Expression<Func<T, bool>> Invert<T>(Expression<Func<T, bool>> e)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.Not(e.Body), e.Parameters[0]);
    }

    /// <summary>
    /// Returns (resultExpression, rootExpression) that allows to get member you need or default value if any of submember on the path will be null.
    /// </summary>
    internal static (Expression, ParameterExpression) GetNestedMemberOrDefaultExpression<TIn, TOut>(
            Expression<Func<TIn, TOut>> memberAccessExpression,
            TOut defaultValue,
            ParameterExpression newParameterExpression = default!)
    {
        Defend.Against.Null(memberAccessExpression, nameof(memberAccessExpression));
        Defend.Against.InvalidMemberAccessChain(memberAccessExpression, out var castedMemberAccessExpression);

        (var memberAccessors, var expression) = SplitMemberAccessChain(castedMemberAccessExpression);

        ParameterExpression rootExpression = newParameterExpression
            ?? expression as ParameterExpression
            ?? throw new Exception($"Invalid member access expression");

        return (MakeConditionalExpression(rootExpression, defaultValue, memberAccessors), rootExpression);
    }

    /// <summary>
    /// Returns MemberExpression that allows to get member you need or default value if any of submember on the path will be null.
    /// </summary>
    internal static MemberExpression ApplyMemberAccessChainToOtherExpression(
            Expression memberAccessExpression,
            Expression newRootExpression)
    {
        Defend.Against.Null(memberAccessExpression, nameof(memberAccessExpression));
        Defend.Against.Null(newRootExpression, nameof(newRootExpression));
        Defend.Against.InvalidMemberAccessChain(memberAccessExpression, out var castedMemberAccessExpression, nameof(memberAccessExpression));

        (var memberAccessors, _) = SplitMemberAccessChain(castedMemberAccessExpression);

        var newMemberAccessChain = newRootExpression;
        foreach (var memberAccessor in memberAccessors)
        {
            newMemberAccessChain = Expression.MakeMemberAccess(newMemberAccessChain, memberAccessor.Member);
        }

        return (MemberExpression)newMemberAccessChain;
    }

    private static Expression MakeConditionalExpression<TOut>(
        Expression expression, TOut defaultValue, IEnumerable<MemberExpression> memberExpressions)
    {
        if (!memberExpressions.Any())
        {
            return expression;
        }

        var testIsNullExpression = Expression.Equal(expression, Expression.Constant(null!, expression.Type));
        var ifTrueExpression = Expression.Constant(defaultValue, typeof(TOut));
        var ifFalseExpression = MakeConditionalExpression(
            Expression.MakeMemberAccess(expression, memberExpressions.First().Member),
            defaultValue,
            memberExpressions.Skip(1));
        return Expression.Condition(testIsNullExpression, ifTrueExpression, ifFalseExpression);
    }

    /// <summary>
    /// Returns member accessors chain and root expression which not member access.
    /// </summary>
    private static (IEnumerable<MemberExpression>, Expression?) SplitMemberAccessChain(Expression expression)
    {
        Defend.Against.InvalidMemberAccessChain(expression, out var castedMemberAccessExpression);

        var memberAccessors = new List<MemberExpression>();
        Expression? _expression = castedMemberAccessExpression;
        while (_expression is MemberExpression castedExpression)
        {
            memberAccessors.Add(castedExpression);
            _expression = castedExpression.Expression;
        }
        memberAccessors.Reverse();

        return (memberAccessors, _expression);
    }
}
