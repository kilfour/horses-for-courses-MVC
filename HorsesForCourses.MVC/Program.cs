using System.Text.Json.Serialization;
using HorsesForCourses.Api.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=horsesforcourses.db"));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(null, allowIntegerValues: false)));

builder.Services
    .AddScoped<IAmASuperVisor, DataSupervisor>()

    .AddScoped<IGetCoachById, GetCoachById>()
    .AddScoped<IGetTheCoachSummaries, GetCoachSummaries>()
    .AddScoped<IGetTheCoachDetail, GetCoachDetail>()

    .AddScoped<IGetCourseById, GetCourseById>()
    .AddScoped<IGetTheCourseSummaries, GetCourseSummaries>()
    .AddScoped<IGetTheCourseDetail, GetCourseDetail>()

    .AddScoped<CoachesRepository>()
    .AddScoped<CoursesRepository>(); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
