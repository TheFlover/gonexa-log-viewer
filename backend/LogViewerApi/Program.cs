using LogViewerApi.Services;
using LogViewerApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:8080") // Adjust this if your Vue app runs on a different port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(nameof(LogSettings)));

builder.Services.AddSingleton<ILogService, LogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS globally
app.UseCors();

app.MapGet("/api/logs", async (ILogService logService) =>
    Results.Ok(await logService.GetAllLogsAsync()))
    .WithName("GetLogs")
    .WithOpenApi();

app.Run();
