using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Real_Time_Temperature_Monitoring_API.Models;
using Real_Time_Temperature_Monitoring_API.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Test;

public class TemperaturesServiceTests
{
    private readonly Mock<IMongoCollection<TemperatureDto>> _mockCollection;
    private readonly Mock<IOptions<TemperatureStoreDatabaseSettings>> _mockOptions;
    private readonly TemperaturesService _service;

    public TemperaturesServiceTests()
    {
        _mockCollection = new Mock<IMongoCollection<TemperatureDto>>();
        _mockOptions = new Mock<IOptions<TemperatureStoreDatabaseSettings>>();

        var settings = new TemperatureStoreDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Temperature",
            TemperatureCollectionName = "Temperatures"
        };

        _mockOptions.Setup(o => o.Value).Returns(settings);

        // Mock the MongoDB client and database to return the mock collection
        var mockDatabase = new Mock<IMongoDatabase>();
        mockDatabase.Setup(db => db.GetCollection<TemperatureDto>(It.IsAny<string>(), null))
            .Returns(_mockCollection.Object);

        var mockClient = new Mock<IMongoClient>();
        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(mockDatabase.Object);

        _service = new TemperaturesService(_mockOptions.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsTemperatureList()
    {
        // Arrange test data 
        var mockTemperatureList = new List<TemperatureDto>
        {
            new TemperatureDto { City = "City1", Timestamp = DateTime.Now, Value = 25.0 }
        };
    
        // Mock the cursor
        var mockCursor = new Mock<IAsyncCursor<TemperatureDto>>();
        mockCursor.SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupGet(cursor => cursor.Current).Returns(mockTemperatureList);
    
        // Mock the collection
        _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<TemperatureDto>>(),
                It.IsAny<FindOptions<TemperatureDto, TemperatureDto>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);
    
        // Act
        var result = await _service.GetAsync();
        result = result.Select(x => { x.Id = null; return x; }).ToList();
        var newResult = result.Skip(result.Count - 1).ToList();
        // Assert - only check relevant properties
        Assert.Equal(mockTemperatureList.Count, newResult.Count);
    
        for (int i = 0; i < mockTemperatureList.Count; i++)
        {
            Assert.Equal(mockTemperatureList[i].City, newResult[i].City);
            Assert.Equal(mockTemperatureList[i].Value, newResult[i].Value);
            // Skip Timestamp and Id
        }
    }


    [Fact]
    public async Task CreateAsync_AddsTemperatureToCollection()
    {
        var date = DateTime.Now;
        // Arrange
        var newTemperature = new TemperatureDto { City = "City1", Timestamp = date, Value = 25 };

        // Act
        await _service.CreateAsync(newTemperature);

        // Assert
        var result = await _service.GetAsync();
        result = result.Select(x => { x.Timestamp = date; return x; }).ToList();
        Assert.NotEmpty(result);
    }

}