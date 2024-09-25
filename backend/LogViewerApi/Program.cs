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
            .WithExposedHeaders("Content-Disposition", "Content-Type");
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
{
    try
    {
        return Results.Ok(await logService.GetAllLogsAsync());
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred.");
    }
})
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
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (FileNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred.");
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
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (FileNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred.");
    }
})
.WithName("DownloadModelFile")
.WithOpenApi();

app.MapPost("/api/logs/{logId}/retry", async (string logId, ILogService logService) =>
{
    try
    {
        var updatedLog = await logService.RetryGenerationAsync(logId);
        return Results.Ok(updatedLog);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred.");
    }
})
.WithName("RetryGeneration")
.WithOpenApi();

app.Run();