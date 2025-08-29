
using System.Text.Json.Serialization;
using HorsesForCourses.Api;
using HorsesForCourses.Service;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHorsesForCourses();
builder.Services.AddHorsesForCoursesWebApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<DomainExceptionMiddleware>();
app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
public partial class Program { }