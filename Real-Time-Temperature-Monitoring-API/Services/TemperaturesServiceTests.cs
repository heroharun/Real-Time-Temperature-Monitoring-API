using Moq;
using MongoDB.Driver;
using Real_Time_Temperature_Monitoring_API.Models;
using Real_Time_Temperature_Monitoring_API.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;

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
            DatabaseName = "test_db",
            TemperatureCollectionName = "test_temperatures"
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
        // Arrange
        var mockTemperatureList = new List<TemperatureDto>
        {
            new TemperatureDto { City = "City1", Timestamp = DateTime.Now, Value = 25.0 },
            new TemperatureDto { City = "City2", Timestamp = DateTime.Now, Value = 30.0 }
        };

        var mockCursor = new Mock<IAsyncCursor<TemperatureDto>>();
        mockCursor.SetupSequence(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupGet(cursor => cursor.Current).Returns(mockTemperatureList);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<TemperatureDto>>(),
            It.IsAny<FindOptions<TemperatureDto, TemperatureDto>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
