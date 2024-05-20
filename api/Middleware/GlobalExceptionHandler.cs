using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using api.DTOs;
using exceptions;

namespace api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            _logger.LogError(exception, "Unhandled exception for request {RequestPath}: {ExceptionMessage}", context.Request.Path, exception.Message);

            context.Response.StatusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                Exceptions.DatabaseConnectionException => StatusCodes.Status503ServiceUnavailable,
                Exceptions.QueryExecutionException => StatusCodes.Status500InternalServerError,
                Exceptions.LoggingQueryExecutionException => StatusCodes.Status500InternalServerError,
                Exceptions.InvalidAccountDataException => StatusCodes.Status400BadRequest,
                Exceptions.InvalidTokenException => StatusCodes.Status401Unauthorized,
                Exceptions.PasswordHashingException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ResponseDto
            {
                MessageToClient = GetClientMessage(exception)
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static string GetClientMessage(Exception exception)
        {
            return exception switch
            {
                ValidationException => "Validation failed for one or more entities.",
                KeyNotFoundException => "The specified resource was not found.",
                AuthenticationException => "Authentication failed.",
                UnauthorizedAccessException => "Access denied.",
                Exceptions.DatabaseConnectionException => "Service currently unavailable. Please try again later.",
                Exceptions.QueryExecutionException => "A problem occurred processing your request.",
                Exceptions.LoggingQueryExecutionException => "There was an error in logging the action.",
                Exceptions.InvalidAccountDataException => "Invalid account data provided.",
                Exceptions.InvalidTokenException => "Invalid token provided.",
                Exceptions.PasswordHashingException => "An error occurred while processing your request.",
                _ => "An unexpected error occurred. We are working to address this issue."
            };
        }
    }
}
