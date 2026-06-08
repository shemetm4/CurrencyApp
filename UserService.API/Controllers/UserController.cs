using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using System.Security.Claims;
using UserService.API.Contracts;
using UserService.Application.Commands.AddFavorite;
using UserService.Application.Commands.RegisterUser;
using UserService.Application.Commands.RemoveFavorite;
using UserService.Application.Interfaces;
using UserService.Application.Queries.LoginUser;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    IRegisterUserHandler registerHandler,
    ILoginUserHandler loginHandler,
    IAddFavoriteHandler addFavoriteHandler,
    IRemoveFavoriteHandler removeFavoriteHandler,
    ITokenBlacklistRepository blacklistRepository) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        await registerHandler.HandleAsync(command);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
    {
        var token = await loginHandler.HandleAsync(query);

        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await CheckBlacklistAsync();

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        await blacklistRepository.AddAsync(token, expiresAt);

        return Ok();
    }

    [Authorize]
    [HttpPost("favorites/add")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
    {
        await CheckBlacklistAsync();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await addFavoriteHandler.HandleAsync(new AddFavoriteCommand(userId, request.CurrencyId));

        return Ok();
    }

    [Authorize]
    [HttpDelete("favorites/remove")]
    public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteRequest request)
    {
        await CheckBlacklistAsync();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await removeFavoriteHandler.HandleAsync(new RemoveFavoriteCommand(userId, request.CurrencyId));

        return Ok();
    }

    private async Task CheckBlacklistAsync()
    {
        var token = Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && await blacklistRepository.IsBlacklistedAsync(token))
            throw new UnauthorizedAccessException("Token has been revoked.");
    }
}
