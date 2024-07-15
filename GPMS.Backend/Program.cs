using FluentValidation.AspNetCore;
using GPMS.Backend;
using GPMS.Backend.Data;
using GPMS.Backend.Middlewares;
using GPMS.Backend.Services.Utils.Validators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
//Add Serilog 
Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(configuration)
.CreateLogger();
// Add services to the container.
builder.Services.ConfigureService(configuration);
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Use Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
