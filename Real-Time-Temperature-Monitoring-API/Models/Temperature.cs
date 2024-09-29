using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Real_Time_Temperature_Monitoring_API.Models;

public class TemperatureDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    
    [BsonElement("City")]
    [JsonPropertyName("city") ]
    public string City { get; set; } = null!;
    
    [BsonElement("Temperature")]
    [JsonPropertyName("temperature")]
    public double Value { get; set; }
    
    [BsonElement("Timestamp")]
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    public ActionResult<TemperatureResponseDto> ToTemperatureResponseDto()
    {
        return new TemperatureResponseDto
        {
            City = City,
            Timestamp = Timestamp,
            Temperature = Value
        };
    }

    public bool Valiadate()
    {
        return !string.IsNullOrEmpty(City) && Timestamp != default;
    }
}

// created DTO for temperature as input
public class TemperatureInputDto
{
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

// created DTO For average temperature
public class TemperatureAverageDto
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("average")]
    public double Average { get; set; }
    [JsonPropertyName("min")]
    public double Min { get; set; }
    [JsonPropertyName("max")]
    public double Max { get; set; }
}

// created DTO for response of latest temperature
public class TemperatureResponseDto
{
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}

// created DTO for response of temperature extremes
public class TemperatureExtremesDto
{
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;
    [JsonPropertyName("highest")]
    public double Highest { get; set; }
    [JsonPropertyName("lowest")]
    public double Lowest { get; set; }
}
