namespace Elasticsearch.API.Endpoints;

public static class ProductEndpoints
{
    public static void AddProductEndpoints(
        this IEndpointRouteBuilder app)
    {
        var productRouteGroup = app
            .MapGroup("/api/products")
            .WithName("ProductEndpoints");

        productRouteGroup.MapGet("/", () => "Hello world!");
    }
}
