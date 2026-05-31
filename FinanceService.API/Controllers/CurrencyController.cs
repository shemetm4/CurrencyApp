using FinanceService.API.Contracts;
using FinanceService.Application.Queries.GetCurrencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyController(GetCurrenciesHandler getCurrenciesHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var currencies = await getCurrenciesHandler.HandleAsync(new GetCurrenciesQuery(userId));
        var response = currencies.Select(c => new GetCurrenciesResponse(c.Id, c.Name, c.ExchangeRate));
        return Ok(response);
    }
}
