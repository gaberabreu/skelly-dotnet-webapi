using Skelly.WebApi.Presentation.Configurations;
using Skelly.WebApi.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLoggerConfigs();

builder.Services.AddControllersConfigs();
builder.Services.AddHttpClient();
builder.Services.AddServiceConfigs();
builder.Services.AddAuthConfigs(builder.Configuration);
builder.Services.AddSwaggerConfigs();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseLoggerConfigs();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfigs();
}

app.UseExceptionHandler(_ => { });

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
