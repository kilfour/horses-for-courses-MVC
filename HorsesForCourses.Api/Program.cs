
using System.Text.Json.Serialization;
using HorsesForCourses.Api;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=horsesforcourses.db"));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(null, allowIntegerValues: false)));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IAmASuperVisor, DataSupervisor>()

    .AddScoped<IGetCoachById, GetCoachById>()
    .AddScoped<IGetTheCoachSummaries, GetCoachSummaries>()
    .AddScoped<IGetTheCoachDetail, GetCoachDetail>()

    .AddScoped<IGetCourseById, GetCourseById>()
    .AddScoped<IGetTheCourseSummaries, GetCourseSummaries>()
    .AddScoped<IGetTheCourseDetail, GetCourseDetail>()

    .AddScoped<CoachesRepository>()
    .AddScoped<CoursesRepository>();


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