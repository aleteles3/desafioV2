using Application.User.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Service.User.Controllers;

public partial class UserController
{
    [HttpPost]
    [Route("AuthToken")]
    public async Task<IActionResult> GenerateUserAuthToken(UserAuthViewModel userAuthViewModel)
    {
        var result = await _userAppService.GenerateUserAuthToken(userAuthViewModel);

        return Response(result);
    }
}