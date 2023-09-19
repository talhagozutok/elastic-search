using System.Net;
using Elasticsearch.API.Dtos;
using Elasticsearch.API.Repositories;
using Elasticsearch.API.Requests;

namespace Elasticsearch.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var responseProduct = await _productRepository
                             .SaveAsync(request.ToProduct());

        if (responseProduct is null)
        {
            return ResponseDto<ProductDto>.Fail(
                new List<string>() { "An error occurred while saving data." },
                HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(
            responseProduct.ToDto(),
            HttpStatusCode.Created);
    }
}
