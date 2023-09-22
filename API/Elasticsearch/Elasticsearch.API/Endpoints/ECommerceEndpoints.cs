using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Endpoints;

public static class ECommerceEndpoints
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder app)
    {
        var routeGroup = app.MapGroup("/api/ecommerce");

        routeGroup.MapGet("/termQuery", GetTermQueryAsync);
        routeGroup.MapPost("/termsQuery", GetTermsQueryAsync);
        routeGroup.MapGet("/prefixQuery", GetPrefixQueryAsync);
        routeGroup.MapGet("/rangeQuery", GetRangeQueryAsync);
        routeGroup.MapGet("/matchAll", GetMatchAllQueryAsync);
        routeGroup.MapGet("/paginationQuery", GetPaginationQueryAsync);
        routeGroup.MapGet("/wildcardQuery", GetWildcardQueryAsync);
        routeGroup.MapGet("/fuzzyQuery", GetFuzzyQueryAsync);
        routeGroup.MapGet("/matchQuery", GetMatchQueryAsync);
    }

    private static async Task<IResult> GetTermQueryAsync(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermQueryAsync(customerFirstName));

    private static async Task<IResult> GetTermsQueryAsync(
        List<string> customerFirstNameList,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermsQueryAsync(customerFirstNameList));

    private static async Task<IResult> GetPrefixQueryAsync(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.PrefixQueryAsync(customerFirstName));

    private static async Task<IResult> GetRangeQueryAsync(
        double? gte, double? lte,
        ECommerceRepository repository)
        => Results.Ok(await repository.RangeQueryAsync(gte, lte));

    private static async Task<IResult> GetMatchAllQueryAsync(
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchAllQueryAsync());

    private static async Task<IResult> GetPaginationQueryAsync(
        ECommerceRepository repository,
        int page = 1, int pageSize = 5)
        => Results.Ok(await repository.PaginationQueryAsync(page, pageSize));

    private static async Task<IResult> GetWildcardQueryAsync(
        string customerFullNameWildcard,
        ECommerceRepository repository)
        => Results.Ok(await repository.WildcardQueryAsync(customerFullNameWildcard));
    
    private static async Task<IResult> GetFuzzyQueryAsync(
        string customerFirstNameWildcard,
        ECommerceRepository repository)
        => Results.Ok(await repository.FuzzyQueryAsync(customerFirstNameWildcard));

    private static async Task<IResult> GetMatchQueryAsync(
        string categoryName,
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchQueryAsync(categoryName));
}
