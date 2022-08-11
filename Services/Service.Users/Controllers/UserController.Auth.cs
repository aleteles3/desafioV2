using Application.Users.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Service.User.Controllers;

public partial class UserController
{
    [HttpPost]
    [Route("AuthToken")]
    public async Task<IActionResult> GenerateUserAuthToken([FromBody] UserAuthViewModel userAuthViewModel)
    {
        var result = await _userAppService.GenerateUserAuthToken(userAuthViewModel);

        return Response(result);
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshTokenViewModel)
    {
        var result = await _userAppService.RefreshToken(refreshTokenViewModel);

        return Response(result);
    }
}