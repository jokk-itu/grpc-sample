using Serilog;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllers();
services.AddGrpc();
services.AddSerilog();

var serilogConfiguration = configuration.GetSection("Serilog");
Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .Enrich.WithProcessName()
  .Enrich.WithProcessId()
  .Enrich.WithThreadName()
  .Enrich.WithThreadId()
  .Enrich.WithMemoryUsage()
  .Enrich.WithProperty("ContainerId", Environment.GetEnvironmentVariable("HOSTNAME"))
  .WriteTo.Console()
  .WriteTo.Seq(serilogConfiguration.GetSection("Seq").GetValue<string>("Url"))
  .CreateBootstrapLogger();

try
{
  var app = builder.Build();
  app.UseSerilogRequestLogging();
  app.UseAuthorization();
  app.MapGrpcService<WeatherService>();
  app.Run();
}
catch(Exception e)
{
  Log.Error(e, "Unexpected error occurred");
}
finally
{
  await Log.CloseAndFlushAsync();
}