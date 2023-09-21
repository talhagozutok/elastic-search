using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Endpoints;

public static class ECommerceEndpoints
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder app)
    {
        var routeGroup = app.MapGroup("/api/ecommerce");

        routeGroup.MapGet("/termQuery", GetTermQuery);
        routeGroup.MapPost("/termsQuery", GetTermsQuery);
        routeGroup.MapGet("/prefixQuery", GetPrefixQuery);
    }

    private static async Task<IResult> GetTermQuery(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermQueryAsync(customerFirstName));

    private static async Task<IResult> GetTermsQuery(
        List<string> customerFirstNameList,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermsQueryAsync(customerFirstNameList));

    private static async Task<IResult> GetPrefixQuery(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.PrefixQueryAsync(customerFirstName));
}
