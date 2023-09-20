using Elasticsearch.API.Requests;
using Elasticsearch.API.Services;

namespace Elasticsearch.API.Endpoints;

public static class ProductEndpoints
{
    public static void AddProductEndpoints(
        this IEndpointRouteBuilder app)
    {
        var productRouteGroup = app
            .MapGroup("/api/products");

        productRouteGroup.MapPost("/", SaveAsync);
    }

    private static async Task<IResult> SaveAsync(
        ProductCreateDto request,
        ProductService productService)
        => Results.Ok(await productService.SaveAsync(request));
}
