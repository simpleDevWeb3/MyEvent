global using MyEvent.Models;
global using MyEvent;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core DbContext
builder.Services.AddSqlServer<DB>($@"
    Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename={builder.Environment.ContentRootPath}\DB.mdf;
    Integrated Security=True;");

// Enable MVC and JSON fix for circular references
builder.Services.AddScoped<MyEvent.Helper>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GeoService>(); //cheng added!!!*************************************************


builder.Services.AddAuthentication().AddCookie();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();             // ? Needed for [Route("api/...")] endpoints

app.MapDefaultControllerRoute();  // Optional if using MVC Views

app.Run();
