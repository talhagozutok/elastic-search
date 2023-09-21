
using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Endpoints;

public static class ECommerceEndpoints
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder app)
    {
        var routeGroup = app.MapGroup("/api/ecommerce");

        routeGroup.MapGet("/termQuery", GetTermQuery);
    }

    private static async Task<IResult> GetTermQuery(
        string customerFirstName,
        ECommerceRepository repository)
        => Results.Ok(await repository.TermQuery(customerFirstName));
}
