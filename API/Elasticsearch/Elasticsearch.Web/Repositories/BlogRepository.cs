using Elastic.Clients.Elasticsearch;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Repositories;

public class BlogRepository
{
    private const string IndexName = "blog";
    private readonly ElasticsearchClient _elasticClient;

    public BlogRepository(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Blog?> SaveAsync(Blog blog)
    {
        blog.CreatedOn = DateTime.Now;

        var response = await _elasticClient.IndexAsync(
            blog,
            x => x.Index(IndexName));

        if (!response.IsValidResponse)
        {
            return null;
        }

        return blog;
    }
}
