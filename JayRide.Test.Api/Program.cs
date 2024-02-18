using CorrelationId;
using CorrelationId.DependencyInjection;
using JayRide.Test.Api.Core;
using JayRide.Test.Api.Core.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.TryUseSerilogFileLogging(builder.Configuration["LoggingFilePath"]);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddDefaultCorrelationId();

builder.Services.AddCoreConfiguration(builder.Configuration)
                .AddCoreServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCorrelationId();

app.UseConfigureExceptionHandling(app.Environment);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();