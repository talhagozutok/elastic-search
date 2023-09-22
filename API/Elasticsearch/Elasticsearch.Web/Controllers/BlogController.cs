using Elasticsearch.Web.Services;
using Elasticsearch.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Web.Controllers;
public class BlogController : Controller
{
    private readonly BlogService _blogService;

    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<IActionResult> Search()
    {
        return View(await _blogService.SearchAsync(string.Empty));
    }

    public IActionResult Save()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromForm] string searchText)
    {
        ViewData["searchText"] = searchText;
        return View(await _blogService.SearchAsync(searchText));
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(BlogCreateViewModel model)
    {
        var isSaveSuccess = await _blogService.SaveAsync(model);

        if (!isSaveSuccess)
        {
            TempData["result"] = "An error occurred when saving blog.";
            return RedirectToAction(nameof(Save));
        }

        TempData["result"] = "Blog saved successfully.";
        return View();
    }
}
