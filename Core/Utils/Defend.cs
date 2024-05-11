using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace Core.Utils;

public static class Defend
{
    public static IGuardClause Against => Guard.Against;
}

public static class DefendExtensions
{
    /// <summary>
    /// Returns default value if argument is null.
    /// Default value is null for reference types and default value for value types.
    /// </summary>
    /// <param name="_">Guard close.</param>
    /// <param name="input">INpu</param>
    /// <param name="defaultValue">Custom default value may be provided if needed.</param>
    /// <returns>The value.</returns>
    public static T NullOrReturnDefault<T>(
        this IGuardClause _,
        T? input,
        T defaultValue = default!)
    {
        return input ?? defaultValue;
    }

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if  any of <paramref name="input"/> elements is null. Pass checks if <paramref name="input"/> is null itself.
    /// </summary>
    public static void NullElements<T>(
       this IGuardClause _,
       IEnumerable<T> input,
       [CallerArgumentExpression(nameof(input))] string? parameterName = null,
       string? message = null)
    {
        if (input is null)
        {
            return;
        }
        if (input.Any(x => x == null))
        {
            throw new ArgumentException(message ?? $"All elements of input {parameterName} must be not null.", parameterName);
        }
    }

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="input"/> is null OR count of <paramref name="input"/> elements is less than <paramref name="minimalElementsQuantity"/>.
    /// </summary>
    public static void LessElementsQuantity<T>(
        this IGuardClause _,
        IEnumerable<T> input,
        long minimalElementsQuantity,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null,
        string? message = null)
    {
        Defend.Against.Null(parameterName, nameof(parameterName));
        Defend.Against.Negative(minimalElementsQuantity, nameof(minimalElementsQuantity));
        Defend.Against.Null(input, parameterName);

        if (input.LongCount() < minimalElementsQuantity)
        {
            throw new ArgumentException(message ?? $"Input {parameterName} must contains at least {minimalElementsQuantity} elements.", parameterName);
        }
    }

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="input"/> is not ordered as Queryable.
    /// </summary>
    public static void NotOrderedQuery(
        this IGuardClause _,
        IQueryable input,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null,
        string? message = null)
    {
        Defend.Against.Null(parameterName, nameof(parameterName));
        Defend.Against.Null(input, parameterName);

        if (ReflectionUtils.IsQueryOrdered(input) == false)
        {
            throw new ArgumentException(message ?? $"Input {parameterName} must be ordered.", parameterName);
        }
    }

    #region InvalidDateTimeRange

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="to"/> is less than <paramref name="from"/>.
    /// </summary>
    public static void InvalidDateTimeRange(
        this IGuardClause _,
        DateTime from,
        DateTime to,
        [CallerArgumentExpression(nameof(from))] string? fromParameterName = null,
        [CallerArgumentExpression(nameof(to))] string? toParameterName = null,
        string? message = null)
    {
        ThrowIfToLessThanFrom(from, to, fromParameterName, toParameterName, message);
    }

    /// <inheritdoc cref="InvalidDateTimeRange(IGuardClause, DateTime, DateTime, string?, string?, string?)"/>
    public static void InvalidDateTimeRange(
        this IGuardClause _,
        DateTimeOffset from,
        DateTimeOffset to,
        [CallerArgumentExpression(nameof(from))] string? fromParameterName = null,
        [CallerArgumentExpression(nameof(to))] string? toParameterName = null,
        string? message = null)
    {
        ThrowIfToLessThanFrom(from, to, fromParameterName, toParameterName, message);
    }

    /// <inheritdoc cref="InvalidDateTimeRange(IGuardClause, DateTime, DateTime, string?, string?, string?)"/>
    public static void InvalidDateTimeRange(
        this IGuardClause _,
        DateOnly from,
        DateOnly to,
        [CallerArgumentExpression(nameof(from))] string? fromParameterName = null,
        [CallerArgumentExpression(nameof(to))] string? toParameterName = null,
        string? message = null)
    {
        ThrowIfToLessThanFrom(from, to, fromParameterName, toParameterName, message);
    }

    /// <inheritdoc cref="InvalidDateTimeRange(IGuardClause, DateTime, DateTime, string?, string?, string?)"/>
    public static void InvalidDateTimeRange(
        this IGuardClause _,
        TimeOnly from,
        TimeOnly to,
        [CallerArgumentExpression(nameof(from))] string? fromParameterName = null,
        [CallerArgumentExpression(nameof(to))] string? toParameterName = null,
        string? message = null)
    {
        ThrowIfToLessThanFrom(from, to, fromParameterName, toParameterName, message);
    }

    private static void ThrowIfToLessThanFrom<T>(T from, T to, string? fromParameterName, string? toParameterName, string? message = null)
        where T : IComparable<T>
    {
        if (to.CompareTo(from) < 0)
        {
            throw new ArgumentException(message ?? $"Date {fromParameterName} must be less or equal than date {toParameterName}.", fromParameterName);
        }
    }

    #endregion

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="expressionToTest"/> is not valid member access chain. Returns as "out" parameter valid member access chain.
    /// </summary>
    public static void InvalidMemberAccessChain(
        this IGuardClause _,
        Expression expressionToTest,
        out MemberExpression validMemberExpression,
        [CallerArgumentExpression(nameof(expressionToTest))] string? parameterName = null,
        string? message = null)
    {
        Defend.Against.Null(expressionToTest, nameof(expressionToTest));

        var expression = expressionToTest;
        if (expressionToTest is LambdaExpression castedLambdaExpression)
        {
            expression = castedLambdaExpression.Body;
        }

        var initExpression = expression;
        while(expression!.NodeType != ExpressionType.Parameter)
        {
            if (expression is not MemberExpression castedMemberAccessExpression)
            {
                throw new ArgumentException(message ?? "Provided member access chain is invalid.", parameterName);
            }
            expression = castedMemberAccessExpression.Expression;
        }

        validMemberExpression = (MemberExpression)initExpression;
    }

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="expressionToTest"/> is not valid member access chain.
    /// </summary>
    public static void InvalidMemberAccessChain(
        this IGuardClause _,
        Expression expressionToTest,
        [CallerArgumentExpression(nameof(expressionToTest))] string? parameterName = null,
        string? message = null)
    {
        Defend.Against.Null(expressionToTest, nameof(expressionToTest));
        Defend.Against.InvalidMemberAccessChain(expressionToTest, out var _, parameterName, message);
    }
}
