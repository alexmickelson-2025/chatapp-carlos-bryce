using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://0.0.0.0:8080");
string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new Exception("API_URL environment variable not set");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

app.MapGet("/", () => "healthy");

app.MapControllers();
app.Run();
