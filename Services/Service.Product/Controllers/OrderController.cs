using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace Service.Product.Controllers;

[Authorize]
[Route("api/Product/Order")]
public class OrderController : CoreController
{
    private readonly IOrderAppService _orderAppService;

    public OrderController(IOrderAppService orderAppService, IMemoryBus memoryBus) : base(memoryBus)
    {
        _orderAppService = orderAppService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] AddOrderViewModel addOrderViewModel)
    {
        await _orderAppService.AddOrder(addOrderViewModel);

        return Response();
    }
}