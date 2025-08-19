global using MyEvent.Models;
global using MyEvent;
using MyEvent.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core DbContext
builder.Services.AddSqlServer<DB>($@"
    Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename={builder.Environment.ContentRootPath}\DB.mdf;
    Integrated Security=True;");

// Enable MVC and JSON fix for circular references
builder.Services.AddScoped<MyEvent.Helper>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/login";            
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

builder.Services.AddAuthentication().AddCookie();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapDefaultControllerRoute();  // Optional if using MVC Views

app.Run();
