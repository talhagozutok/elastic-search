using System.Globalization;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Services;

public class ECommerceService
{
    private readonly ECommerceRepository _repository;

    public ECommerceService(ECommerceRepository repository)
    {
        _repository = repository;
    }

    public async Task<(List<ECommerceViewModel>, long totalCount, long pageCount)> SearchAsync(
        int page,
        int pageSize,
        ECommerceSearchViewModel viewModel)
    {
        var (eCommerceList, totalCount) = await _repository.SearchAsync(
            page,
            pageSize,
            viewModel);

        var pageCountCalculate = totalCount % pageSize;
        long pageCount = 0;

        if (pageCountCalculate == 0)
        {
            pageCount = totalCount / pageSize;
        }
        else
        {
            pageCount = (totalCount / pageSize) + 1;
        }

        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        var eCommerceListViewModel = eCommerceList.Select(x => new ECommerceViewModel()
        {
            CustomerFirstName = x.CustomerFirstName,
            CustomerLastName = x.CustomerLastName,
            CustomerFullName = x.CustomerFullName,
            Gender = ti.ToTitleCase(x.Gender),
            OrderId = x.OrderId,
            OrderDate = x.OrderDate.ToShortDateString(),
            TaxfulTotalPrice = x.TaxfulTotalPrice,
            Category = string.Join(",", x.Category),
        });

        return (eCommerceListViewModel.ToList(),
            totalCount,
            pageCount);
    }
}
