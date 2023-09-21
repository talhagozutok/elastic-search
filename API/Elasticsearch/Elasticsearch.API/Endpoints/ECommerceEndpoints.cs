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
    }

    private static async Task<IResult> GetTermQuery(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermQuery(customerFirstName));

    private static async Task<IResult> GetTermsQuery(
        List<string> fieldValues,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermsQuery(fieldValues));
}
