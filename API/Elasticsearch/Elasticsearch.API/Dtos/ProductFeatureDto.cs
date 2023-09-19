using Elasticsearch.API.Models;

namespace Elasticsearch.API.Requests;

public record ProductFeatureDto(
    int Width,
    int Height,
    Color Color);
