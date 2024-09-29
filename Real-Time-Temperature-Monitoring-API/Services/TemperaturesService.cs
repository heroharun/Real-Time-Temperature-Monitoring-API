using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Real_Time_Temperature_Monitoring_API.Models;

namespace Real_Time_Temperature_Monitoring_API.Services;

public class TemperaturesService
{
    private readonly IMongoCollection<TemperatureDto> _temperaturesCollection;

    // <snippet_ctor>
    public TemperaturesService(
        IOptions<TemperatureStoreDatabaseSettings> temperatureStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            temperatureStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            temperatureStoreDatabaseSettings.Value.DatabaseName);

        _temperaturesCollection = mongoDatabase.GetCollection<TemperatureDto>(
            temperatureStoreDatabaseSettings.Value.TemperatureCollectionName);
    }
    // </snippet_ctor>

    public async Task<List<TemperatureDto>> GetAsync() =>
        await _temperaturesCollection.Find(_ => true).ToListAsync();
    
    public async Task<List<TemperatureDto>> GetAsync(string city, DateTime start, DateTime end) =>
        await _temperaturesCollection.Find(x => x.City == city && x.Timestamp >= start && x.Timestamp <= end).ToListAsync();

    
    public async Task<List<TemperatureDto>> GetAsyncByCity(string city) =>
        await _temperaturesCollection.Find(x => x.City == city ).ToListAsync();
    
    public async Task CreateAsync(TemperatureDto newTemperature) =>
        await _temperaturesCollection.InsertOneAsync(newTemperature);
    
}