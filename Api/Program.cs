using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.WebHost.UseUrls("http://0.0.0.0:8080");
string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new Exception("CONNECTION_STRING environment variable not set");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

if (!Directory.Exists("wwwroot/images"))
{
    Directory.CreateDirectory("wwwroot/images");
}
 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")),
    RequestPath = "/api/wwwroot/images"
});


app.MapGet("/", () => "healthy");
app.MapGet("/api", () => "something else");

app.MapControllers();
app.Run();
