using API.Attributes;
using DTOs.Weather;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController()
        {
        }

        [HttpGet]
        [Route("/weather")]
        public List<WeatherForecastListItemDTO> Get()
        {
            return Enumerable.Range(1, 20).Select(index => new WeatherForecastListItemDTO
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
        }
    }
}
