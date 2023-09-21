using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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

    public async Task<ImmutableList<ECommerce>> TermQueryAsync(
        string customerFirstName)
    {
        /*
         * Equivalent firstName query example.
         * 
         * GET /kibana_sample_data_ecommerce/_search
         *  {
         *    "query": {
         *      "firstName": {
         *        "customer_first_name.keyword": {
         *          "value": "customerFirstName"
         *        }
         *      }
         *    }
         *  }
         *
         */

        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(ECommerceIndexName)
            .Query(q => q
                .Term(
                   f => f.CustomerFirstName.Suffix("keyword"),
                   customerFirstName)));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> TermsQueryAsync(
        List<string> customerFirstNameList)
    {
        var terms = new List<FieldValue>();
        customerFirstNameList.ForEach(x => terms.Add(x));

        var termsQuery = new TermsQuery()
        {
            Field = "customer_first_name.keyword",
            Terms = new TermsQueryField(terms.AsReadOnly())
        };

        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(ECommerceIndexName)
            .Query(termsQuery));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(
        string customerFirstName)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(ECommerceIndexName)
            .Query(q => q
                .Prefix(p => p
                    .Field(f => f.CustomerFirstName.Suffix("keyword"))
                    .Value(customerFirstName))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> RangeQueryAsync(
        double? gte, double? lte)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(ECommerceIndexName)
                .Query(q => q
                    .Range(r => r
                        .NumberRange(
                            nr => nr.Field(f => f.TaxfulTotalPrice)
                            .Gte(gte).Lte(lte)))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(ECommerceIndexName)
            .Query(q => q
                .MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
}
