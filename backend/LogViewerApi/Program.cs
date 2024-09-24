using LogViewerApi.Services;
using LogViewerApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services for API documentation and CORS configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow requests from http://localhost:8080 origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition");
    });
});

// Bind LogSettings configuration and register logging service
builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(nameof(LogSettings)));

builder.Services.AddSingleton<ILogService, LogService>();

var app = builder.Build();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Define API route to fetch all logs
app.MapGet("/api/logs", async (ILogService logService) =>
    Results.Ok(await logService.GetAllLogsAsync()))
    .WithName("GetLogs")
    .WithOpenApi();

// Define API route to download generated log file by ID
app.MapGet("/api/logs/{logId}/download/generatedfile", async (string logId, ILogService logService) =>
{
    try
    {
        var (fileStream, contentType, fileName) = await logService.DownloadGeneratedFileAsync(logId);
        return Results.File(fileStream, contentType, fileName);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"Log with ID {logId} not found.");
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (FileNotFoundException)
    {
        return Results.NotFound($"Generated file for log ID {logId} not found.");
    }
})
.WithName("DownloadGeneratedFile")
.WithOpenApi();

// Define API route to download model log file by ID
app.MapGet("/api/logs/{logId}/download/modelfile", async (string logId, ILogService logService) =>
{
    try
    {
        var (fileStream, contentType, fileName) = await logService.DownloadModelFileAsync(logId);
        return Results.File(fileStream, contentType, fileName);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"Log with ID {logId} not found.");
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (FileNotFoundException)
    {
        return Results.NotFound($"Model file for log ID {logId} not found.");
    }
})
.WithName("DownloadModelFile")
.WithOpenApi();

app.Run();