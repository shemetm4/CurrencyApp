using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using UserService.Domain.Exceptions;

namespace UserService.API;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case UserAlreadyExistsException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;
            case InvalidCredentialsException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            case CurrencyNotExistsException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case CurrencyAlreadyInFavoriteException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;
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
