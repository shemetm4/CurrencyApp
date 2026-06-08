using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace FinanceService.API.Middleware;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case UnauthorizedAccessException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                logger.LogError(exception, "An unexpected exception occurred.");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsync(exception.Message, cancellationToken);

        return true;
    }
}
