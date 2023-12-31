﻿using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace Elasticsearch.API.Repositories;

public class ECommerceRepository
{
    private readonly ElasticsearchClient _elasticClient;

    private const string IndexName = "kibana_sample_data_ecommerce";
    private const string KeywordSuffix = "keyword";

    public ECommerceRepository(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    #region Term-level queries
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
            .Index(IndexName)
            .Query(q => q
                .Term(
                   f => f.CustomerFirstName.Suffix(KeywordSuffix),
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
            .Index(IndexName)
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
            .Index(IndexName)
            .Query(q => q
                .Prefix(p => p
                    .Field(f => f.CustomerFirstName.Suffix(KeywordSuffix))
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
            .Index(IndexName)
            .Query(q => q
                .Range(r => r
                    .NumberRange(nr => nr
                        .Field(f => f.TaxfulTotalPrice)
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
            .Index(IndexName)
            .Query(q => q
                .MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(
        int page, int pageSize)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(pageSize).From((page - 1) * pageSize)
            .Query(q => q.MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> WildcardQueryAsync(
        string customerFullNameWildcard)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .Wildcard(p => p
                    .Field(f => f.CustomerFullName.Suffix(KeywordSuffix))
                    .Value(customerFullNameWildcard))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(
        string customerFirstName)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .Fuzzy(p => p
                    .Field(f => f.CustomerFirstName.Suffix(KeywordSuffix))
                    .Value(customerFirstName)))
            .Sort(s => s
                .Field(f => f.TaxfulTotalPrice,
                new FieldSort() { Order = SortOrder.Desc })));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
    #endregion

    #region Full text queries
    public async Task<ImmutableList<ECommerce>> MatchQueryAsync(
        string categoryName)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .Match(p => p
                    .Field(f => f.Category)
                    .Query(categoryName))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MultiMatchQueryAsync(
        string name)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .MultiMatch(q => q
                    .Query(name)
                    .Fields(new Field("customer_first_name")
                                .And("customer_last_name")
                                .And("customer_full_name")))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchBoolPrefixAsync(
        string customerFullName)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .MatchBoolPrefix(p => p
                    .Field(f => f.CustomerFullName)
                    .Query(customerFullName))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchPhraseQueryAsync(
        string customerFullName)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .MatchPhrase(p => p
                    .Field(f => f.CustomerFullName)
                    .Query(customerFullName))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> CompoundQueryAsync(
        string cityName,
        double taxfulTotalPriceLte,
        string categoryKeyword,
        string manufacturerKeyword)
    {
        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Query(q => q
                .Bool(b => b
                    .Must(bq => bq.Term(f => f.CityName, cityName))
                    .MustNot(bq => bq.Range(r => r.NumberRange(rq => rq.Field(f => f.TaxfulTotalPrice).Lte(taxfulTotalPriceLte))))
                    .Should(bq => bq.Term(f => f.Category.Suffix(KeywordSuffix), categoryKeyword))
                    .Filter(bq => bq.Term(f => f.Manufacturer.Suffix(KeywordSuffix), manufacturerKeyword)))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
    #endregion
}
