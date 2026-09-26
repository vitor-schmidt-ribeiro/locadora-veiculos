using System.Net;
using System.Text.Json;

namespace LocadoraVeiculos.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate _next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this._next = _next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção não tratada na requisição.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var resposta = new
        {
            status = context.Response.StatusCode,
            mensagem = "Ocorreu um erro interno no servidor ao processar a solicitação.",
            detalhe = exception.Message
        };

        var json = JsonSerializer.Serialize(resposta);
        return context.Response.WriteAsync(json);
    }
}
