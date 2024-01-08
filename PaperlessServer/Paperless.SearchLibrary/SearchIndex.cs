namespace Paperless.SearchLibrary;

using System.Collections.Generic;
using System.Diagnostics;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class ElasticSearchIndex : ISearchIndex
{
    private readonly Uri _uri;
    private readonly ILogger<ElasticSearchIndex> _logger;

    public ElasticSearchIndex(IConfiguration configuration, ILogger<ElasticSearchIndex> logger)
    {
        _uri = new Uri(configuration.GetConnectionString("ElasticSearch") ?? "http://elastic_search:9200/");
        _logger = logger;
    }
    public void AddDocumentAsync(Document document)
    {
        var elasticClient = new ElasticsearchClient(_uri);

        if (!elasticClient.Indices.Exists("documents").Exists)
        {
            elasticClient.Indices.Create("documents");
        }

        var indexResponse = elasticClient.Index(document, "documents");
        
        if (!indexResponse.IsSuccess())
        {
            // Handle errors
            _logger.LogError($"Failed to index document: {indexResponse.DebugInformation}\n{indexResponse.ElasticsearchServerError}");

            throw new Exception($"Failed to index document: {indexResponse.DebugInformation}\n{indexResponse.ElasticsearchServerError}");
        }
    }

    public async Task<IEnumerable<Document>> SearchDocumentAsync(string searchTerm, int? limit)
    {
        var elasticClient = new ElasticsearchClient(_uri);

        var fuzzinessLevel = searchTerm.Length switch
        {
            <= 2 => "0",
            <= 5 => "1",
            _ => "2"
        };
        /*
                var searchResponse = await elasticClient.SearchAsync<Document>(s => s
                    .Index("documents")
                    .Size(limit ?? 10)
                    .Query(q => q
                        .Fuzzy(fz => fz
                            .Field(f => f.Content)
                            .Value(searchTerm)
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                );
        */

        var searchResponse = elasticClient.Search<Document>(s => s
            .Index("documents")
            .Query(q => q.QueryString(qs => qs.DefaultField(p => p.Content).Query($"*{searchTerm}*")))
        );


        if (!searchResponse.IsSuccess())
        {
            _logger.LogError($"Elasticsearch search error: {searchResponse.DebugInformation}\n{searchResponse.ElasticsearchServerError}");
            throw new Exception($"Search operation failed: {searchResponse.DebugInformation}\n{searchResponse.ElasticsearchServerError}");
        }

        return searchResponse.Documents;
    }
}


