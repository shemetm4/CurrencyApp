using FinanceService.Application.Queries.GetCurrencies;
using FinanceService.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using System.Security.Claims;

namespace FinanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyController(
    IGetCurrenciesHandler getCurrenciesHandler,
    ITokenBlacklistRepository blacklistRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        await CheckBlacklistAsync();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var currencies = await getCurrenciesHandler.HandleAsync(new GetCurrenciesQuery(userId));

        var response = currencies.Select(c => new GetCurrenciesResponse(c.Id, c.Name, c.ExchangeRate));

        return Ok(response);
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
