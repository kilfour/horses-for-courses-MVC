using HorsesForCourses.Api;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace HorsesForCourses.Tests.Miscellaneous;

public class DomainExceptionMiddlewareTests
{
    [Fact]
    public async Task When_DomainException_thrown_returns_400_problem_details()
    {
        RequestDelegate next = _ => throw new OverlappingTimeSlots();
        var logger = Mock.Of<ILogger<DomainExceptionMiddleware>>();
        var mw = new DomainExceptionMiddleware(next, logger);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();


        await mw.Invoke(ctx);

        Assert.Equal(StatusCodes.Status400BadRequest, ctx.Response.StatusCode);
        ctx.Response.Body.Position = 0;
        using var reader = new StreamReader(ctx.Response.Body);
        var json = await reader.ReadToEndAsync();
        Assert.Contains("\"title\":\"Domain rule violated\"", json);
        Assert.Contains("\"detail\":\"Overlapping time slots.\"", json);
        Assert.Contains("\"status\":400", json);
    }
}