using System.Net;
using Library.Core.DTO;
using Library.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (IncorrectDataException ex)
        {
            await HandleExceptionAsync(ex, httpContext, HttpStatusCode.BadRequest);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleExceptionAsync(ex, httpContext, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, httpContext, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(Exception ex, HttpContext context, HttpStatusCode httpStatusCode)
    {
        logger.LogError(ex.Message);
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        var errorDto = new ResponseDto<object>(
            message: ex.Message,
            isSuccess: false,
            data: null,
            details: ex.StackTrace ?? string.Empty
        );

        await response.WriteAsJsonAsync(errorDto);
    }
}