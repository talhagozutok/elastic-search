using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Repositories;

public class BlogRepository
{
    private readonly ElasticsearchClient _elasticClient;

    private const string IndexName = "blog";

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

        List<Action<QueryDescriptor<Blog>>> queryDescriptors = new();

        Action<QueryDescriptor<Blog>> matchAll = q => q.MatchAll();

        Action<QueryDescriptor<Blog>> matchContent = s => s
            .Match(m => m.Field(f => f.Content)
                         .Query(searchText));

        Action<QueryDescriptor<Blog>> matchTitle = s => s
            .MatchBoolPrefix(m => m.Field(f => f.Title)
                                   .Query(searchText));

        Action<QueryDescriptor<Blog>> matchTag = s => s
            .Term(f => f.Tags, searchText);

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

        if (string.IsNullOrEmpty(searchText))
        {
            queryDescriptors.Add(matchAll);
        }
        else
        {
            queryDescriptors.Add(matchContent);
            queryDescriptors.Add(matchTitle);
            queryDescriptors.Add(matchTag);
        }

        var result = await _elasticClient.SearchAsync<Blog>(s => s
            .Index(IndexName)
                .Query(q => q
                    .Bool(bq => bq.Should(queryDescriptors.ToArray()))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToList();
    }
}
