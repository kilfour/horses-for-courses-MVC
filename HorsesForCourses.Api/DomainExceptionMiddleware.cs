using System.Text.RegularExpressions;
using HorsesForCourses.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api;

public sealed class DomainExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<DomainExceptionMiddleware> logger;

    public DomainExceptionMiddleware(RequestDelegate next, ILogger<DomainExceptionMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogInformation(ex, "Domain rule violated");
            await WriteProblem(context, StatusCodes.Status400BadRequest, "Domain rule violated", GetDetailMessage(ex));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static string GetDetailMessage(DomainException exception)
    {
        var type = exception.GetType().Name;
        // Insert a space before each capital (except the first one)
        string withSpaces = Regex.Replace(type, "(?<!^)([A-Z])", " $1");
        return $"{new string([.. withSpaces.Take(1)])}{new string([.. withSpaces.Skip(1).Select(char.ToLower)])}.";

    }

    private static async Task WriteProblem(HttpContext ctx, int status, string title, string detail)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = status switch
            {
                StatusCodes.Status400BadRequest => "https://httpstatuses.com/400",
                StatusCodes.Status404NotFound => "https://httpstatuses.com/404",
                StatusCodes.Status409Conflict => "https://httpstatuses.com/409",
                _ => "about:blank"
            }
        };

        await ctx.Response.WriteAsJsonAsync(problem);
    }
}