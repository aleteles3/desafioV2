using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Service.Product.Controllers;

[Route("api/Product/Category")]
public class CategoryController : Controller
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    [Route("GetCategoryById")]
    public async Task<IActionResult> GetCategoryById([FromQuery] Guid id)
    {
        var result = await _categoryAppService.GetCategoryById(id);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterViewModel categoryFilterViewModel,
        [FromQuery] int? start = null, [FromQuery] int? length = null)
    {
        var result = await _categoryAppService.GetCategories(categoryFilterViewModel,
            start, length);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryViewModel addCategoryViewModel)
    {
        var result = await _categoryAppService.AddCategory(addCategoryViewModel);

        return result != null ? Ok(result) : BadRequest(result);
    }
}