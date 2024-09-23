using LogViewerApi.Services;
using LogViewerApi.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.MapGet("/api/logs", async (ILogService logService) =>
    Results.Ok(await logService.GetAllLogsAsync()))
    .WithName("GetLogs")
    .WithOpenApi();

app.Run();
