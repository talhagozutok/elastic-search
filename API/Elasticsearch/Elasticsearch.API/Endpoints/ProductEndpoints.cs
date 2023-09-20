using System.Net;
using Elasticsearch.API.Dtos;
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
        productRouteGroup.MapPut("/", UpdateAsync);
        productRouteGroup.MapGet("/", GetAllAsync);
        productRouteGroup.MapGet("/{id}", GetByIdAsync);
    }

    private static async Task<IResult> SaveAsync(
        ProductCreateDto request,
        ProductService productService)
        => Results.Ok(await productService.SaveAsync(request));

    private static async Task<IResult> UpdateAsync(
        ProductUpdateDto updateDto,
        ProductService productService)
    {
        var response = await productService.UpdateAsync(updateDto);

        if (response.StatusCode is HttpStatusCode.InternalServerError)
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }

        return Results.NoContent();
    }

    private static async Task<IResult> GetAllAsync(
        ProductService productService)
        => Results.Ok(await productService.GetAllAsync());

    private static async Task<IResult> GetByIdAsync(
        string id,
        ProductService productService)
    {
        if (id is null)
        {
            return Results.NotFound();
        }

        var productResponse = await productService.GetByIdAsync(id);

        if (productResponse.StatusCode is HttpStatusCode.NotFound)
        {
            return Results.NotFound();
        }

        return Results.Ok(productResponse);
    }
}
