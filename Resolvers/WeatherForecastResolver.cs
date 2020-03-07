using HotChocolate;
using SampleApp.Models;

namespace SampleApp.Resolvers
{
    internal class WeatherForecastResolver
    {
        public string GetSummary([Parent] WeatherForecast forecast)
        {
            if (forecast.TemperatureC <= 0)
            {
                return "Freezing";
            }
            else if (forecast.TemperatureC > 0 && forecast.TemperatureC <= 10)
            {
                return "Chilly";
            }
            else if (forecast.TemperatureC > 10 && forecast.TemperatureC <= 20)
            {
                return "Mild";
            }
            else if (forecast.TemperatureC > 20 && forecast.TemperatureC <= 24)
            {
                return "Pleasant";
            }
            else if (forecast.TemperatureC > 24 && forecast.TemperatureC <= 30)
            {
                return "Warm";
            }
            else
            {
                return "Hot";
            }
        }
    }
}