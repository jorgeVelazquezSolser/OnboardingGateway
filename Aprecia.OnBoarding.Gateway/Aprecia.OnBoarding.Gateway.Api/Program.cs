using Aprecia.DI.Gateway.Test.Extensions;
using Aprecia.DI.Gateway.Authorization.Extensions;
using Aprecia.DI.Gateway.SalesExecutive.Extensions;
using Serilog;
using Serilog.Debugging;
using Aprecia.DI.Gateway.People.Estensions;

var builder = WebApplication.CreateBuilder(args);

// Logs
SelfLog.Enable(Console.Out);
SelfLog.Enable(Console.Error);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.ConfigureTestDependencies(builder.Configuration);
builder.Services.ConfigureAuthorizationDependencies(builder.Configuration); 
builder.Services.ConfigureSalesExecutiveDependencies(builder.Configuration);
builder.Services.ConfigurePeopleDependencies(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<SerilogLoggingMiddleware>();
//app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
