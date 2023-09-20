using System.Collections.Immutable;
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

        var productDto = responseProduct.ToDto();

        return ResponseDto<ProductDto>.Success(
            productDto,
            HttpStatusCode.Created);
    }

    public async Task<ImmutableList<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products
            .Select(
            x => new ProductDto(
                x.Id,
                x.Name,
                x.Price,
                x.Stock,
                new ProductFeatureDto(
                    x.Feature?.Width,
                    x.Feature?.Height,
                    x.Feature?.Color?.ToString("g"))))
            .ToImmutableList();
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var hasProduct = await _productRepository.GetByIdAsync(id);

        if (hasProduct is null)
        {
            return ResponseDto<ProductDto>.Fail(
                "Product not found.",
                HttpStatusCode.NotFound);
        }

        var productDto = hasProduct.ToDto();

        return ResponseDto<ProductDto>.Success(
            productDto,
            HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateDto)
    {
        var isSuccess = await _productRepository.UpdateAsync(updateDto);

        if (!isSuccess)
        {
            return ResponseDto<bool>.Fail(
                "An error occurred when updating product.",
                HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var isSuccess = await _productRepository.DeleteAsync(id);

        if (!isSuccess)
        {
            return ResponseDto<bool>.Fail(
                "An error occurred when deleting product.",
                HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }
}
