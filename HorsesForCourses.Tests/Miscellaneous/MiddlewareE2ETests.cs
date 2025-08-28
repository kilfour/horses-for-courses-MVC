using System.Net;
using HorsesForCourses.Api;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace HorsesForCourses.Tests.Miscellaneous;

public class MiddlewareE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    public readonly WebApplicationFactory<Program> factory;

    public MiddlewareE2ETests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder =>
        {
            builder.Configure(app =>
            {
                app.UseMiddleware<DomainExceptionMiddleware>();
                app.Map("/boom", a => a.Run(ctx => throw new CourseAlreadyConfirmed()));
            });
        });
    }

    [Fact]
    public async Task DomainException_is_mapped_to_400()
    {
        var client = factory.CreateClient();
        var resp = await client.GetAsync("/boom");
        var body = await resp.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        Assert.Contains("\"title\":\"Domain rule violated\"", body);
        Assert.Contains("\"detail\":\"Course already confirmed.\"", body);
    }
}
