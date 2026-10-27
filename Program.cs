var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
var app = builder.Build();

app.MapGet("/", (IConfiguration c) => new
{
    version  = Environment.GetEnvironmentVariable("APP_VERSION"),
    revision = Environment.GetEnvironmentVariable("CONTAINER_APP_REVISION"),   // có giá trị khi chạy trên Container Apps
    host     = Environment.MachineName,
    message  = c["Api:Message"] ?? "(chưa cấu hình)"
});
app.MapGet("/env", (IConfiguration c) => new
{
    message = c["Api:Message"],
    hasDb   = !string.IsNullOrEmpty(c.GetConnectionString("Db")),
    hasPass = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PASS"))
});
app.MapHealthChecks("/healthz");
app.Run();
// trigger
