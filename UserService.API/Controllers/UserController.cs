using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.API.Contracts;
using UserService.Application.Commands.AddFavorite;
using UserService.Application.Commands.RegisterUser;
using UserService.Application.Commands.RemoveFavorite;
using UserService.Application.Queries.LoginUser;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    RegisterUserHandler registerHandler,
    LoginUserHandler loginHandler,
    AddFavoriteHandler addFavoriteHandler,
    RemoveFavoriteHandler removeFavoriteHandler) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await registerHandler.HandleAsync(command);
        if (!result)
            return Conflict("User with this name already exists.");
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
    {
        var token = await loginHandler.HandleAsync(query);
        if (token is null)
            return Unauthorized();
        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok();
    }

    // todo: correct response for not existing currencies 
    [Authorize]
    [HttpPost("favorites/add")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await addFavoriteHandler.HandleAsync(new AddFavoriteCommand(userId, request.CurrencyId));
        return Ok();
    }

    [Authorize]
    [HttpDelete("favorites/remove")]
    public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await removeFavoriteHandler.HandleAsync(new RemoveFavoriteCommand(userId, request.CurrencyId));
        return Ok();
    }
}
