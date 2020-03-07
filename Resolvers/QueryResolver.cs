using System;
using System.Collections.Generic;
using System.Linq;
using SampleApp.Models;

namespace SampleApp.Resolvers
{
    public class QueryResolver
    {
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
            })
            .ToArray();
        }
    }
}