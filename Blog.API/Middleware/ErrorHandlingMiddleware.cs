
using System.ComponentModel.DataAnnotations;
using Blog.API.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logService;
    private readonly IWebHostEnvironment _environment;

    private record HttpStatusCodeErrorCode(int HttpStatusCode, ErrorCode ErrorCode);
    private record ErrorResponse(string ErrorCode, string Message);
    private static readonly Dictionary<Type, HttpStatusCodeErrorCode> _exceptionStatusCodes = new()
    {
        [typeof(NotImplementedException)] = new(StatusCodes.Status501NotImplemented, ErrorCode.INTERNAL_SERVER_ERROR),
        [typeof(UnauthorizedAccessException)] = new(StatusCodes.Status401Unauthorized, ErrorCode.UNAUTHORIZED),
        [typeof(SecurityTokenExpiredException)] = new(StatusCodes.Status401Unauthorized, ErrorCode.UNAUTHORIZED),
        [typeof(SecurityTokenValidationException)] = new(StatusCodes.Status401Unauthorized, ErrorCode.UNAUTHORIZED),
        [typeof(ValidationException)] = new(StatusCodes.Status400BadRequest, ErrorCode.VALIDATION_ERROR),
        //[typeof(BadRequestException)] = new(StatusCodes.Status400BadRequest, ErrorCode.BAD_REQUEST),
        [typeof(DbUpdateException)] = new(StatusCodes.Status400BadRequest, ErrorCode.DB_ERROR),
        //[typeof(NotFoundException)] = new(StatusCodes.Status404NotFound, ErrorCode.NOT_FOUND),
        //[typeof(DependencyException)] = new(StatusCodes.Status424FailedDependency, ErrorCode.DEPENDENCY_ERROR),
        [typeof(NotSupportedException)] = new(StatusCodes.Status409Conflict, ErrorCode.DEPENDENCY_ERROR),
    };

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logService,
        IWebHostEnvironment environment
        )
    {
        _next = next;
        _logService = logService;
        _environment = environment;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var exceptionMessage = exception.InnerException?.Message ?? exception.Message;

        _logService.LogError(
            exception,
            "Unhandled exception for user {UserName} from {IpAddress}: {Method} {Path}",
            context.User.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value,
            context.Connection.RemoteIpAddress?.ToString(),
            context.Request?.Method,
            context.Request?.Path);
        
        var exceptionType = exception.GetType();
        var errorMap = _exceptionStatusCodes.ContainsKey(exceptionType)
            ? _exceptionStatusCodes[exceptionType]
            : new(StatusCodes.Status500InternalServerError, ErrorCode.INTERNAL_SERVER_ERROR);

        context.Response.StatusCode = errorMap.HttpStatusCode;
        context.Response.ContentType = "application/json";

        // The real exception message is only useful (and safe) to expose
        // outside Production — in Production it could leak internal
        // details (as it did here: a domain type's full name), same
        // environment-gating reasoning already used for Swagger
        // (learning-notes/notes/35-environments-config.md).
        var message = _environment.IsProduction()
            ? "An unexpected error occurred."
            : exceptionMessage;

        return context.Response.WriteAsJsonAsync(new ErrorResponse(errorMap.ErrorCode.ToString(), message));
    }
}