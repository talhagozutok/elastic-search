using System.Net;
using Elasticsearch.API.Dtos;
using Elasticsearch.API.Services;

namespace Elasticsearch.API.Endpoints;

public static class ProductEndpoints
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder app)
    {
        var routeGroup = app.MapGroup("/api/products");

        routeGroup.MapPost("/", SaveAsync);
        routeGroup.MapPut("/", UpdateAsync);
        routeGroup.MapGet("/", GetAllAsync);
        routeGroup.MapGet("/{id}", GetByIdAsync);
        routeGroup.MapDelete("/{id}", DeleteAsync);
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

    private static async Task<IResult> DeleteAsync(
        string id,
        ProductService productService)
    {
        var response = await productService.DeleteAsync(id);

        if (response.StatusCode is HttpStatusCode.InternalServerError)
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return Results.NotFound(response);
        }

        return Results.NoContent();
    }
}
