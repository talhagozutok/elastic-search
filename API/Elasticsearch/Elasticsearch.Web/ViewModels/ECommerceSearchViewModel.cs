using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.Web.ViewModels;

public class ECommerceSearchViewModel
{
    public string? Category { get; set; }
    public string? Gender { get; set; }

    [DisplayName("Customer Full Name")]
    public string? CustomerFullName { get; set; }

    [DisplayName("Order Date (Start)")]
    [DataType(DataType.Date)]
    public DateTime? OrderDateStart { get; set; }

    [DisplayName("Order Date (End)")]
    [DataType(DataType.Date)]
    public DateTime? OrderDateEnd { get; set; }
}
