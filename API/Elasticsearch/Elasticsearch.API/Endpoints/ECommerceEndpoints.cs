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

    private static async Task<IResult> GetMatchAllQueryAsync(ECommerceRepository repository)
        => Results.Ok(await repository.MatchAllQueryAsync());
}
