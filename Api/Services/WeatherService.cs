using Api.Contracts.Weather;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Api.Services;
 
public class WeatherService : Weather.WeatherBase
{
  private readonly static string[] WeatherNames = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
  private const int Floor = -50;
  private const int Ceiling = 51;

  public override Task<WeatherResponse> GetCountryWeather(WeatherRequest request, ServerCallContext context)
  {
    return GetWeather();
  }

  public override async Task GetWeather(Empty request, IServerStreamWriter<WeatherResponse> responseStream, ServerCallContext context)
  {
    for(var i = 0; i < Random.Shared.Next(0, 51) && !context.CancellationToken.IsCancellationRequested; ++i)
    {
      await Task.Delay(100);
      await responseStream.WriteAsync(await GetWeather());
    }
  }

  private static Task<WeatherResponse> GetWeather()
  {
    return Task.FromResult(new WeatherResponse
    {
      Name = WeatherNames[Random.Shared.Next(0, WeatherNames.Length)],
      Temperature = Random.Shared.Next(Floor, Ceiling)
    });
  }
}