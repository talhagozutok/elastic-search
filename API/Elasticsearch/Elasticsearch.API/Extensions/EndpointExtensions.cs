using Elasticsearch.API.Endpoints;

namespace Elasticsearch.API.Extensions;

public static class EndpointExtensions
{
    public static void MapAllEndpoints(
        this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseEndpoints(ProductEndpoints.AddEndpoints);
        applicationBuilder.UseEndpoints(ECommerceEndpoints.AddEndpoints);
    }
}
