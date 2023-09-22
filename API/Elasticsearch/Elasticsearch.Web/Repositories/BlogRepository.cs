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

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        // should ( (term1 or term2) or term3 )

        /*
         * // .net 5 ile gelen yenilikler
         * POST /blog/_search
         *  {
         *    "query": {
         *      "bool": {
         *        "should": [
         *          {
         *            "match": {
         *              "title": "yenilik"
         *            }
         *          },
         *          {
         *            "match_bool_prefix": {
         *              "title": "yenilik"
         *            }
         *          }
         *        ]
         *      }
         *    }
         *  }
         * 
         */

        var result = await _elasticClient.SearchAsync<Blog>(s => s
            .Index(IndexName)
                .Query(q => q
                    .Bool(bq => bq
                        .Should(
                            s => s.Match(m => m.Field(f => f.Content).Query(searchText)),
                            s => s.MatchBoolPrefix(m => m.Field(f => f.Title).Query(searchText))))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToList();
    }
}
