using Ardalis.GuardClauses;
using Core.EntityFramework.Features.SearchPagination.Interfaces;
using Core.EntityFramework.Features.SearchPagination.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.EntityFramework.Features.SearchPagination;

public enum SearchCasePolicy
{
    CaseSensitive,
    CaseInsensitive,
}
public enum SearchTextMatchPolicy
{
    FullMatch,
    PartialMatch,
}

public static class QueryableExtensions
{
    /// <summary>
    /// Add pagination to query.
    /// Query must be ordered before using this method.
    /// </summary>
    /// <returns>Query.</returns>
    public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query, IPagedQuery pagedQuery)
    {
        Defend.Against.NotOrderedQuery(query);

        if (pagedQuery is null)
        {
            return query;
        }

        if (pagedQuery.Offset.HasValue)
        {
            query = query.Skip(pagedQuery.Offset.Value);
        }
        if (pagedQuery.Limit.HasValue)
        {
            query = query.Take(pagedQuery.Limit.Value);
        }

        return query;
    }

    /// <summary>
    /// Add pagination to the query and enumerate it.<br></br>
    /// Query must be ordered before using this method.
    /// CAUTION: Set <paramref name="applyPagination"/> to false if used <see cref="ApplyPagination{T}(IQueryable{T}, IPagedQuery)"/>, otherwise it will double the pagination.
    /// </summary>
    /// <param name="applyPagination">If true – <see cref="ApplyPagination{T}(IQueryable{T}, IPagedQuery)"/> will be used to the query.</param>
    /// <param name="totalItemsRewrite">
    /// Value to be set as "TotalItems" in <see cref="PagedResult{T}"/>.
    /// Useful if query was already paginated to set actual total items quantity.
    /// Required!!! if <paramref name="applyPagination"/> is false.
    /// </param>
    /// <returns>Paged result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<PagedResult<T>> AsPagedResultAsync<T>(
        this IQueryable<T> query,
        IPagedQuery pagedQuery,
        bool applyPagination = true,
        long? totalItemsRewrite = null!,
        CancellationToken cancellationToken = default)
    {
        Defend.Against.Null(query, nameof(query));
        Defend.Against.Null(pagedQuery, nameof(pagedQuery));
        Defend.Against.NotOrderedQuery(query, nameof(query));
        if (applyPagination == false)
        {
            Defend.Against.Null(totalItemsRewrite, nameof(totalItemsRewrite), message: $"Input {nameof(totalItemsRewrite)} required if {nameof(applyPagination)} is false");
            Defend.Against.Negative(totalItemsRewrite.Value, nameof(totalItemsRewrite));
        }


        var paginatedQueryable = query;
        if (applyPagination)
        {
            paginatedQueryable = query.ApplyPagination(pagedQuery);
        }

        var list = await paginatedQueryable.ToListAsync(cancellationToken);
        return new PagedResult<T>()
        {
            Items = list,
            ItemsOffset = pagedQuery.Offset ?? 0,
            ItemsQuantity = list.Count,
            TotalItems = totalItemsRewrite ?? await query.CountAsync(cancellationToken),
        };
    }

    /// <summary>
    /// Build paged result from already enumerated query. This method will not apply pagination to the query.
    /// </summary>
    /// <param name="totalItemsRewrite">
    /// Value to be set as "TotalItems" in <see cref="PagedResult{T}"/>.
    /// </param>
    /// <returns>Paged result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static PagedResult<T> AsPagedResult<T>(
        this IEnumerable<T> query,
        IPagedQuery pagedQuery,
        long totalItemsRewrite)
    {
        Defend.Against.Null(query, nameof(query));
        Defend.Against.Null(pagedQuery, nameof(pagedQuery));
        Defend.Against.Negative(totalItemsRewrite, nameof(totalItemsRewrite));

        var list = query.ToList();
        return new PagedResult<T>()
        {
            Items = list,
            ItemsOffset = pagedQuery.Offset ?? 0,
            ItemsQuantity = list.Count,
            TotalItems = totalItemsRewrite,
        };
    }

    /// <inheritdoc cref="ApplySearch{T}(IQueryable{T}, string, SearchCasePolicy, SearchTextMatchPolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            ISearchQuery searchQuery,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        return ApplySearch(query, searchQuery, SearchCasePolicy.CaseInsensitive, SearchTextMatchPolicy.PartialMatch, propsToSearch);
    }

    /// <inheritdoc cref="ApplySearch{T}(IQueryable{T}, string, SearchCasePolicy, SearchTextMatchPolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            ISearchQuery searchQuery,
            SearchCasePolicy searchCasePolicy = SearchCasePolicy.CaseInsensitive,
            SearchTextMatchPolicy searchTextMatchPolicy = SearchTextMatchPolicy.PartialMatch,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        if (searchQuery is null || string.IsNullOrEmpty(searchQuery.SearchQuery))
        {
            return query;
        }

        return query.ApplySearch(searchQuery.SearchQuery, searchCasePolicy, searchTextMatchPolicy, propsToSearch);
    }

    /// <summary>
    /// Filter query by search string.<br></br>
    /// You can provide multiple properties to search. Search string will be concatenated from them in the provided order.<br></br>
    /// Works only with string properties. Can be compiled to SQL.<br></br>
    /// Case insensitive, partial match by default.<br></br>
    /// </summary>
    public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string searchQuery,
            SearchCasePolicy searchCasePolicy = SearchCasePolicy.CaseInsensitive,
            SearchTextMatchPolicy searchTextMatchPolicy = SearchTextMatchPolicy.PartialMatch,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        Defend.Against.Null(query, nameof(query));
        Defend.Against.LessElementsQuantity(propsToSearch, 1, nameof(propsToSearch));
        Defend.Against.NullElements(propsToSearch, nameof(propsToSearch));
        foreach (var propToSearch in propsToSearch)
        {
            Defend.Against.InvalidMemberAccessChain(propToSearch, nameof(propToSearch));
        }

        if (string.IsNullOrEmpty(searchQuery))
        {
            return query;
        }

        // Create lambda for Where clause.
        var parameter = Expression.Parameter(typeof(T));

        // Build search string. Using string.Concat to concatenate multiple properties on the DB level.
        var concatMethodInfo = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) })!;
        var delimeterExpression = Expression.Constant(" ");
        var expressionsToConcat = new List<Expression>();
        foreach (var prop in propsToSearch)
        {
            var (safeMemberAccessExpression, _) = ExpressionUtils.GetNestedMemberOrDefaultExpression(prop, string.Empty, parameter);
            expressionsToConcat.Add(safeMemberAccessExpression);
        }

        Expression targetStringExpression = expressionsToConcat[0];
        for (int i = 0; i < expressionsToConcat.Count - 1; i++)
        {
            targetStringExpression = Expression.Add(targetStringExpression, delimeterExpression, concatMethodInfo);
            targetStringExpression = Expression.Add(targetStringExpression, expressionsToConcat[i + 1], concatMethodInfo);
        }

        var searchQueryString = searchQuery.Trim();
        // Make it lower case if needed
        if (searchCasePolicy == SearchCasePolicy.CaseInsensitive)
        {
            searchQueryString = searchQueryString.ToLower();
            targetStringExpression = Expression.Call(targetStringExpression, nameof(string.ToLower), null);
        }

        // Check if it contains searching query string.
        var searchingQueryExpression = Expression.Constant(searchQueryString, typeof(string));
        Expression searchMethodCallExpression = default!;
        if (searchTextMatchPolicy == SearchTextMatchPolicy.PartialMatch)
        {
            searchMethodCallExpression = Expression.Call(targetStringExpression, nameof(string.Contains), null, searchingQueryExpression);
        }
        else if (searchTextMatchPolicy == SearchTextMatchPolicy.FullMatch)
        {
            searchMethodCallExpression = Expression.Equal(targetStringExpression, searchingQueryExpression);
        }

        // Create Where clause lambda.
        var whereLambda = Expression.Lambda(searchMethodCallExpression, parameter);

        // Apply search
        query = query.Where((Expression<Func<T, bool>>)whereLambda);

        return query;
    }

    /// <inheritdoc cref="ApplySearchByTokens{T}(IQueryable{T}, IReadOnlyCollection{string}, SearchCasePolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            ISearchQuery searchQuery,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        if (searchQuery is null || string.IsNullOrEmpty(searchQuery.SearchQuery))
        {
            return query;
        }

        var tokensToSearch = searchQuery.SearchQuery.Split();
        return ApplySearchByTokens(query, tokensToSearch, SearchCasePolicy.CaseInsensitive, propsToSearch);
    }

    /// <inheritdoc cref="ApplySearchByTokens{T}(IQueryable{T}, IReadOnlyCollection{string}, SearchCasePolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            ISearchQuery searchQuery,
            SearchCasePolicy searchCasePolicy = SearchCasePolicy.CaseInsensitive,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        if (searchQuery is null || string.IsNullOrEmpty(searchQuery.SearchQuery))
        {
            return query;
        }

        var tokensToSearch = searchQuery.SearchQuery.Split();
        return ApplySearchByTokens(query, tokensToSearch, searchCasePolicy, propsToSearch);
    }

    /// <inheritdoc cref="ApplySearchByTokens{T}(IQueryable{T}, IReadOnlyCollection{string}, SearchCasePolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            string searchQuery,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            return query;
        }

        var tokensToSearch = searchQuery.Split();
        return ApplySearchByTokens(query, tokensToSearch, SearchCasePolicy.CaseInsensitive, propsToSearch);
    }

    /// <inheritdoc cref="ApplySearchByTokens{T}(IQueryable{T}, IReadOnlyCollection{string}, SearchCasePolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            string searchQuery,
            SearchCasePolicy searchCasePolicy = SearchCasePolicy.CaseInsensitive,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            return query;
        }

        var tokensToSearch = searchQuery.Split();
        return ApplySearchByTokens(query, tokensToSearch, searchCasePolicy, propsToSearch);
    }

    /// <inheritdoc cref="ApplySearchByTokens{T}(IQueryable{T}, IReadOnlyCollection{string}, SearchCasePolicy, Expression{Func{T, string?}}[])"/>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            IReadOnlyCollection<string> tokensToSearch,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        return ApplySearchByTokens(query, tokensToSearch, SearchCasePolicy.CaseInsensitive, propsToSearch);
    }

    /// <summary>
    /// Filter query by every search token.<br></br>
    /// You can provide multiple tokens to search (with "and" operator).
    /// You can provide multiple properties to search. Search string will be concatenated from them in the provided order.<br></br>
    /// Works only with string properties. Can be compiled to SQL.<br></br>
    /// Case insensitive by default.<br></br>
    /// </summary>
    public static IQueryable<T> ApplySearchByTokens<T>(
            this IQueryable<T> query,
            IReadOnlyCollection<string> tokensToSearch,
            SearchCasePolicy searchCasePolicy = SearchCasePolicy.CaseInsensitive,
            params Expression<Func<T, string?>>[] propsToSearch)
    {
        Defend.Against.Null(query, nameof(query));
        Defend.Against.LessElementsQuantity(tokensToSearch, 1, nameof(tokensToSearch));
        Defend.Against.NullElements(tokensToSearch, nameof(tokensToSearch));

        foreach (var token in tokensToSearch)
        {
            query = query.ApplySearch(token, searchCasePolicy, SearchTextMatchPolicy.PartialMatch, propsToSearch);
        }
        return query;
    }
}

