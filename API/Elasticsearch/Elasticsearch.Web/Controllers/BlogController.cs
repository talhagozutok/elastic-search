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

    public IActionResult Save()
    {
        return View();
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
