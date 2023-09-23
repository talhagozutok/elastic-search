namespace Elasticsearch.Web.ViewModels;

public class SearchPageViewModel
{
    public long TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public long PageCount { get; set; }
    public List<ECommerceViewModel> List { get; set; }
    public ECommerceSearchViewModel SearchViewModel { get; set; }

    public string CreatePageUrl(
        long page,
        int pageSize,
        HttpRequest request)
    {
        var currentUrl = new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsoluteUri;

        if (currentUrl.Contains("page", StringComparison.OrdinalIgnoreCase))
        {
            currentUrl = currentUrl.Replace(
                $"Page={Page}", $"Page={page}",
                StringComparison.OrdinalIgnoreCase);

            currentUrl = currentUrl.Replace(
                $"PageSize={PageSize}", $"Page={pageSize}",
                StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            currentUrl = $"{currentUrl}?Page={page}";
            currentUrl = $"{currentUrl}&PageSize={pageSize}";
        }

        return currentUrl;
    }
}
