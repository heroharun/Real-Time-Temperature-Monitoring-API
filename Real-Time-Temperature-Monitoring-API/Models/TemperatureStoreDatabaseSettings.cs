namespace Real_Time_Temperature_Monitoring_API.Models;

public class TemperatureStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    
    public string DatabaseName { get; set; } = null!;
    
    public string TemperatureCollectionName { get; set; } = null!;
}