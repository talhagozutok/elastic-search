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

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        return await _blogRepository.SearchAsync(searchText);
    }
}
