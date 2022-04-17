using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace Service.Product.Controllers;

[Authorize]
[Route("api/Product")]
public class ProductController : CoreController
{
    private readonly IProductAppService _productAppService;
    
    public ProductController(IProductAppService productAppService, IMemoryBus memoryBus) : base(memoryBus)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    [Route("GetProductById")]
    public async Task<IActionResult> GetProductById([FromQuery] Guid id)
    {
        var result = await _productAppService.GetProductById(id);

        return Response(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilterViewModel productFilterViewModel,
        [FromQuery] int? start = null, [FromQuery] int? length = null)
    {
        var result = await _productAppService.GetProducts(productFilterViewModel, start, length);

        return Response(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductViewModel addProductViewModel)
    {
        var result = await _productAppService.AddProduct(addProductViewModel);

        return Response(result, StatusCodes.Status201Created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductViewModel updateProductViewModel)
    {
        await _productAppService.UpdateProduct(updateProductViewModel);

        return Response(successStatusCode: StatusCodes.Status204NoContent);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveProduct([FromQuery] Guid id)
    {
        await _productAppService.RemoveProduct(id);

        return Response(successStatusCode: StatusCodes.Status204NoContent);
    }
}