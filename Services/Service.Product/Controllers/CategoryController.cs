using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Domain.Core.Enums;
using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace Service.Product.Controllers;

[Route("api/Product/Category")]
public class CategoryController : CoreController
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryController(ICategoryAppService categoryAppService, IMemoryBus memoryBus) : base(memoryBus)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    [Route("GetCategoryById")]
    public async Task<IActionResult> GetCategoryById([FromQuery] Guid id)
    {
        var result = await _categoryAppService.GetCategoryById(id);

        return Response(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterViewModel categoryFilterViewModel,
        [FromQuery] int? start = null, [FromQuery] int? length = null)
    {
        var result = await _categoryAppService.GetCategories(categoryFilterViewModel,
            start, length);

        return Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryViewModel addCategoryViewModel)
    {
        var result = await _categoryAppService.AddCategory(addCategoryViewModel);

        return Response(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryViewModel updateCategoryViewModel)
    {
        await _categoryAppService.UpdateCategory(updateCategoryViewModel);
        
        return Response();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveCategory([FromQuery] Guid? id)
    {
        if (id == null)
        {
            _memoryBus.RaiseValidationError(ErrorCode.ServiceValidationError, "Category Id must be informed.");
            
            return Response();
        }

        await _categoryAppService.RemoveCategory(id.Value);
        
        return Response();
    }
}