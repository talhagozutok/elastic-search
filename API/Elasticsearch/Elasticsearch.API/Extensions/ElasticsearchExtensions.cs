﻿using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.API.Extensions;

public static class ElasticsearchExtensions
{
    public static void AddElasticClient(
        this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        var elasticConfigSection = configurationRoot.GetSection("Elastic");
        string username = elasticConfigSection["Username"]!;
        string password = elasticConfigSection["Password"]!;

        var settings = new ElasticsearchClientSettings(
            new Uri(elasticConfigSection["Url"]!))
            .Authentication(new BasicAuthentication(username, password));

        var client = new ElasticsearchClient(settings);

        services.AddSingleton(client);
    }
}
