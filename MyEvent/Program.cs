global using MyEvent.Models;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core DbContext
builder.Services.AddSqlServer<DB>($@"
    Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename={builder.Environment.ContentRootPath}\DB.mdf;
    Integrated Security=True;");

// Enable MVC and JSON fix for circular references
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();             // ? Needed for [Route("api/...")] endpoints
app.MapDefaultControllerRoute();  // Optional if using MVC Views

app.Run();
