// <snippet_UsingModels>
using Real_Time_Temperature_Monitoring_API.Models;
// </snippet_UsingModels>
// <snippet_UsingServices>
using Real_Time_Temperature_Monitoring_API.Services;
// </snippet_UsingServices>

// <snippet_AddControllers>


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TemperatureStoreDatabaseSettings>(
    builder.Configuration.GetSection("TemperatureStoreDatabase"));
// </snippet_BookStoreDatabaseSettings>

builder.Services.AddSingleton<TemperaturesService>();
// </snippet_BooksService>

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// </snippet_AddControllers>

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();