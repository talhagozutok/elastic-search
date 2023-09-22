using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.Web.ViewModels;

public class BlogCreateViewModel
{
    [Required]
    [DisplayName("Blog Title")]
    public string Title { get; set; } = null!;

    [Required]
    [DisplayName("Blog Content")]
    public string Content { get; set; } = null!;

    public string? Tags { get; set; }
}

