using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions;

public static class ElasticsearchExtensions
{
    public static void AddElasticClient(
        this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        var elasticConfigSection = configurationRoot.GetSection("Elastic");
        var pool = new SingleNodeConnectionPool(new Uri(elasticConfigSection["Url"]!));

        var settings = new ConnectionSettings(pool);
        var client = new ElasticClient(settings);

        services.AddSingleton(typeof(ElasticClient), client);
    }
}
