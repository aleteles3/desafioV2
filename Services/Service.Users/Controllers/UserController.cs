using Application.Users.Interfaces;
using Application.Users.ViewModels.Crud;
using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace Service.User.Controllers;

[Route("api/User")]
public partial class UserController : CoreController
{
    private readonly IUserAppService _userAppService;
    public UserController(IUserAppService userAppService, IMemoryBus memoryBus) : base(memoryBus)
    {
        _userAppService = userAppService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserViewModel addUserViewModel)
    {
        var result = await _userAppService.AddUser(addUserViewModel);

        return Response(result, StatusCodes.Status201Created);
    }
}