
using Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace WebApp.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception. TraceId {TraceId}", context.TraceIdentifier);
                await WriteProblemDetailsAsync(context, ex);
            }
        }

        private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
                throw ex;

            var (statusCode, title) = MapException(ex);

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = GetClientSafeDetail(ex),
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private string? GetClientSafeDetail(Exception ex)
        {
            if (_environment.IsDevelopment())
                return ex.Message;

            return ex switch
            {
                DomainException => ex.Message,
                ArgumentException => ex.Message,
                InvalidOperationException => ex.Message,
                KeyNotFoundException => ex.Message,
                _ => null
            };
        }

        private (int statusCode, string title) MapException(Exception ex)
        {
            return ex switch
            {
                DomainException => ((int)HttpStatusCode.BadRequest, "Domain rule violated."),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid request."),
                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "Invalid operation."),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found."),
                DbUpdateConcurrencyException => ((int)HttpStatusCode.Conflict, "Concurrency conflict."),
                DbUpdateException => ((int)HttpStatusCode.Conflict, "Database update failed."),
                _ => ((int)HttpStatusCode.InternalServerError, "Unexpected error.")
            };
        }
    }
}