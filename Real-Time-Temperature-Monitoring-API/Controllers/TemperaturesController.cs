using Microsoft.AspNetCore.Mvc;
using Real_Time_Temperature_Monitoring_API.Models;
using Real_Time_Temperature_Monitoring_API.Services;

namespace Real_Time_Temperature_Monitoring_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperaturesController : ControllerBase
{
    private readonly TemperaturesService _temperatureService;

    public TemperaturesController(TemperaturesService temperatureService) =>
        _temperatureService = temperatureService;

    [HttpGet]
    public async Task<List<TemperatureDto>> Get() =>
        await _temperatureService.GetAsync();
    
    
    // Endpoint: GET /temperatures/{city}
    // Description: Retrieve aggregated temperature data for a specific city within a specified time range.
    [HttpGet("{city:minlength(1)} {start:datetime} {end:datetime}")]
    public async Task<ActionResult<TemperatureAverageDto>> Get(string city, DateTime start, DateTime end)
    {
        var temperature = await _temperatureService.GetAsync(city, start, end);
        
        if (temperature.Count == 0)
        {
            return NotFound();
        }
        
        return CalculateAvarage(temperature);
    }

    private ActionResult<TemperatureAverageDto> CalculateAvarage(List<TemperatureDto> temperature)
    {
        var temperatureAvarage = new TemperatureAverageDto
        {
            Count = temperature.Count,
            Average = temperature.Average(x => x.Value),
            Min = temperature.Min(x => x.Value),
            Max = temperature.Max(x => x.Value)
        };

        return temperatureAvarage;
    }
    
    // Endpoint: GET /temperatures/{city}/latest
    // Description: Retrieve the latest temperature reading for a specific city.
    [HttpGet("{city:minlength(1)}/latest")]
    public async Task<ActionResult<TemperatureResponseDto>> GetLatest(string city)
    {
        var temperature = await _temperatureService.GetAsyncByCity(city);
        
        if (temperature.Count == 0)
        {
            return NotFound();
        }
       
        var latestTemperature = temperature.OrderByDescending
            (x => x.Timestamp).First().ToTemperatureResponseDto();
        
        return latestTemperature;
    }
    
    // Endpoint: GET /temperatures/{city}/extremes
    // Description: Retrieve the highest and lowest temperatures recorded for a specific city.
    [HttpGet("{city:minlength(1)}/extremes")]
    public async Task<ActionResult<TemperatureExtremesDto>> GetExtremes(string city)
    {
        var temperature = await _temperatureService.GetAsyncByCity(city);
        
        if (temperature.Count == 0)
        {
            return NotFound();
        }
        
        var temperatureExtremes = new TemperatureExtremesDto
        {
            City = city,
            Highest = temperature.Max(x => x.Value),
            Lowest = temperature.Min(x => x.Value)
        };

        return temperatureExtremes;
    }

    // Endpoint: POST /temperatures
    // Description: Ingest a new temperature reading for a city.
    // Request Body: JSON object containing temperature information.
    //Response: Status code 200 on success, 400 if the request is malformed.
    [HttpPost]
    public async Task<IActionResult> Post(TemperatureInputDto newTemperature)
    {
        var temperature = new TemperatureDto
        {
            City = newTemperature.City,
            Timestamp = newTemperature.Timestamp,
            Value = newTemperature.Temperature
        };
        await _temperatureService.CreateAsync(temperature);
        return Ok();
    }
    
}