using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Extensions;

public static class ElasticsearchExtensions
{
    public static void AddElasticClient(
        this IServiceCollection services,
        IConfigurationRoot configurationRoot)
    {
        var elasticConfigSection = configurationRoot.GetSection("Elastic");

        var url = elasticConfigSection["Url"]!;
        var userName = elasticConfigSection["Username"]!;
        var password = elasticConfigSection["Password"]!;

        var settings = new ElasticsearchClientSettings(new Uri(url))
            .Authentication(new BasicAuthentication(userName, password));

        var elasticClient = new ElasticsearchClient(settings);

        //ConfigureElasticClient(elasticClient);

        services.AddSingleton(elasticClient);
    }

    public static void ConfigureElasticClient(ElasticsearchClient client)
    {
        AddBlogMapping(client);
    }

    private static void AddBlogMapping(ElasticsearchClient client)
    {
        const string BlogIndexName = "blog";

        var indexExistsResponse = client.Indices.Exists(BlogIndexName);
        if (!indexExistsResponse.Exists)
        {
            var createIndexResponse = client.Indices
                .Create<Blog>(BlogIndexName, c => c
                .Mappings(map => map
                    .Properties(props => props
                        .Text(
                            t => t.Title,
                            f => f.Fields(x => x.Keyword(k => k.Title)))
                        .Text(t => t.Content)
                        .Keyword(k => k.UserId)
                        .Keyword(k => k.Tags)
                        .Date(d => d.CreatedOn))));
        }
    }
}
