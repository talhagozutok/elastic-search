using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.Models.ECommerceModel;

namespace Elasticsearch.API.Repositories;

public class ECommerceRepository
{
    private readonly ElasticsearchClient _elasticClient;
    private const string ECommerceIndexName = "kibana_sample_data_ecommerce";

    public ECommerceRepository(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<ImmutableList<ECommerce>> TermQuery(
        string customerFirstName)
    {
        /*
         * Equivalent term query example.
         * 
         * GET /kibana_sample_data_ecommerce/_search
         *  {
         *    "query": {
         *      "term": {
         *        "customer_first_name.keyword": {
         *          "value": "customerFirstName"
         *        }
         *      }
         *    }
         *  }
         *
         */
        var result = await _elasticClient
            .SearchAsync<ECommerce>
            (s =>
             s.Index(ECommerceIndexName)
                .Query(q =>
                     q.Term(t =>
                        t.Field("customer_first_name.keyword")
                             .Value(customerFirstName))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
}
