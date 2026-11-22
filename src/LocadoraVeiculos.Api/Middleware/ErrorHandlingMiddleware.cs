using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var mensagem = "Ocorreu um erro interno no servidor ao processar a solicitação.";

        if (exception is DbUpdateException dbEx)
        {
            var innerMsg = dbEx.InnerException?.Message ?? dbEx.Message;
            if (innerMsg.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase) ||
                innerMsg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                innerMsg.Contains("2601") || innerMsg.Contains("2627"))
            {
                statusCode = (int)HttpStatusCode.Conflict;
                if (innerMsg.Contains("CPF", StringComparison.OrdinalIgnoreCase))
                    mensagem = "Já existe um cliente cadastrado com este CPF.";
                else if (innerMsg.Contains("Email", StringComparison.OrdinalIgnoreCase))
                    mensagem = "Já existe um cliente cadastrado com este e-mail.";
                else if (innerMsg.Contains("CNH", StringComparison.OrdinalIgnoreCase))
                    mensagem = "Já existe um cliente cadastrado com esta CNH.";
                else if (innerMsg.Contains("Placa", StringComparison.OrdinalIgnoreCase))
                    mensagem = "Já existe um veículo cadastrado com esta placa.";
                else
                    mensagem = "Não foi possível concluir a operação pois já existe um registro com as mesmas informações únicas.";
            }
        }

        context.Response.StatusCode = statusCode;

        var resposta = new
        {
            status = statusCode,
            mensagem = mensagem
        };

        var json = JsonSerializer.Serialize(resposta);
        return context.Response.WriteAsync(json);
    }
}

