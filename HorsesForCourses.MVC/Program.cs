using HorsesForCourses.MVC;
using HorsesForCourses.Service;
using HorsesForCourses.Service.Warehouse;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHorsesForCourses();
builder.Services.AddHorsesForCoursesMVC();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", o => { o.LoginPath = "/Account/Login"; });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CanEditCoach", p =>
        p.RequireClaim("skill", "Agile Coaching"))
    .AddPolicy("CanReallyEditCoach", p =>
        p.RequireClaim("skill", "Very Agile Coaching"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
    }
    app.UseExceptionHandler("/Home/Error");
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
