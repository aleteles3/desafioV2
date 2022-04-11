using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Service.Product.Controllers;

[Route("api/Product")]
public class ProductController : Controller
{
    private readonly IProductAppService _productAppService;
    
    public ProductController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    [Route("GetProductById")]
    public async Task<IActionResult> GetProductById([FromQuery] Guid id)
    {
        var result = await _productAppService.GetProductById(id);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilterViewModel productFilterViewModel,
        [FromQuery] int? start = null, [FromQuery] int? length = null)
    {
        var result = await _productAppService.GetProducts(productFilterViewModel, start, length);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductViewModel addProductViewModel)
    {
        var result = await _productAppService.AddProduct(addProductViewModel);

        return result != null ? Ok(result) : BadRequest(result);
    }
}