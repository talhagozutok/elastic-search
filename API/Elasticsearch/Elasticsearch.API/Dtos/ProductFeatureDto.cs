namespace Elasticsearch.API.Requests;

public record ProductFeatureDto(
    int? Width,
    int? Height,
    string? Color);
