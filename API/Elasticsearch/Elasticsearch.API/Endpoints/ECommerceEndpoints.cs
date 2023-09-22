using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Endpoints;

public static class ECommerceEndpoints
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder app)
    {
        var routeGroup = app.MapGroup("/api/ecommerce");

        routeGroup.MapGet("/termQuery", TermQueryAsync);
        routeGroup.MapPost("/termsQuery", TermsQueryAsync);
        routeGroup.MapGet("/prefixQuery", PrefixQueryAsync);
        routeGroup.MapGet("/rangeQuery", RangeQueryAsync);
        routeGroup.MapGet("/matchAll", MatchAllQueryAsync);
        routeGroup.MapGet("/paginationQuery", PaginationQueryAsync);
        routeGroup.MapGet("/wildcardQuery", WildcardQueryAsync);
        routeGroup.MapGet("/fuzzyQuery", FuzzyQueryAsync);
        routeGroup.MapGet("/matchQuery", MatchQueryAsync);
        routeGroup.MapGet("/matchBoolPrefixQuery", MatchBoolPrefixAsync);
        routeGroup.MapGet("/matchPhraseQuery", MatchPhraseQueryAsync);
        routeGroup.MapGet("/compoundQuery", CompoundQueryAsync);
    }

    private static async Task<IResult> TermQueryAsync(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermQueryAsync(customerFirstName));

    private static async Task<IResult> TermsQueryAsync(
        List<string> customerFirstNameList,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermsQueryAsync(customerFirstNameList));

    private static async Task<IResult> PrefixQueryAsync(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.PrefixQueryAsync(customerFirstName));

    private static async Task<IResult> RangeQueryAsync(
        double? gte, double? lte,
        ECommerceRepository repository)
        => Results.Ok(await repository.RangeQueryAsync(gte, lte));

    private static async Task<IResult> MatchAllQueryAsync(
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchAllQueryAsync());

    private static async Task<IResult> PaginationQueryAsync(
        ECommerceRepository repository,
        int page = 1, int pageSize = 5)
        => Results.Ok(await repository.PaginationQueryAsync(page, pageSize));

    private static async Task<IResult> WildcardQueryAsync(
        string customerFullNameWildcard,
        ECommerceRepository repository)
        => Results.Ok(await repository.WildcardQueryAsync(customerFullNameWildcard));

    private static async Task<IResult> FuzzyQueryAsync(
        string customerFirstNameWildcard,
        ECommerceRepository repository)
        => Results.Ok(await repository.FuzzyQueryAsync(customerFirstNameWildcard));

    private static async Task<IResult> MatchQueryAsync(
        string categoryName,
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchQueryAsync(categoryName));

    private static async Task<IResult> MatchBoolPrefixAsync(
        string customerFullName,
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchBoolPrefixAsync(customerFullName));

    private static async Task<IResult> MatchPhraseQueryAsync(
        string customerFullName,
        ECommerceRepository repository)
        => Results.Ok(await repository.MatchPhraseQueryAsync(customerFullName));

    private static async Task<IResult> CompoundQueryAsync(
        string cityName,
        double taxfulTotalPriceLte,
        string categoryKeyword,
        string manufacturerKeyword,
        ECommerceRepository repository)
        => Results.Ok(await repository.CompoundQueryAsync(
            cityName,
            taxfulTotalPriceLte,
            categoryKeyword,
            manufacturerKeyword));
}
