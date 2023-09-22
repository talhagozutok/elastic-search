using Elasticsearch.Web.Models;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Services;

public class BlogService
{
    private readonly BlogRepository _blogRepository;

    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<bool> SaveAsync(BlogCreateViewModel model)
    {
        var newBlog = new Blog()
        {
            UserId = Guid.NewGuid(),
            Title = model.Title,
            Content = model.Content
        };

        if (model.Tags is not null)
        {
            newBlog.Tags = model.Tags.Split(
                new string[] { ",", ", " },
                StringSplitOptions.TrimEntries).ToList();
        }

        var createdBlog = await _blogRepository.SaveAsync(newBlog);
        var isBlogCreated = createdBlog != null;

        return isBlogCreated;
    }

    public async Task<List<BlogViewModel>> SearchAsync(string searchText)
    {
        var blogList = await _blogRepository.SearchAsync(searchText);

        return blogList.Select(b => new BlogViewModel()
        {
            Id = b.Id,
            Title = b.Title,
            Content = b.Content,
            UserId = b.UserId.ToString(),
            Tags = string.Join(", ", b.Tags),
            CreatedOn = b.CreatedOn.ToShortDateString()
        }).ToList();
    }
}
