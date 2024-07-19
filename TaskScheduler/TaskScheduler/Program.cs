using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using TaskScheduler.Cache;
using TaskScheduler.Configuration;
using TaskScheduler.Service;
using TaskScheduler.Service.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<JobService>();

var redisConnectionString = Environment.GetEnvironmentVariable("Redis_Connection");
if (string.IsNullOrEmpty(redisConnectionString))
{
    throw new InvalidOperationException("The Redis connection string is not set in the environment variables.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddHangfire(options =>
{
    var redis = builder.Services.BuildServiceProvider().GetRequiredService<IConnectionMultiplexer>();
    options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = "HANG_FIRE" });
});
builder.Services.AddHangfireServer();


var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("The TMDb API key is not set in the environment variables.");
}

// Register MovieService with HttpClient and API Key
builder.Services.AddHttpClient<MovieService>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});

builder.Services.AddScoped<MovieService>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(MovieService));
    return new MovieService(httpClient, apiKey);
});

builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Scheduler API",
        Version = "v1",
        Description = "This application is designed to schedule tasks using HangFire with Redis",
        TermsOfService = new Uri("https://www.linkedin.com/in/erickson-de-quadros-cz/"),
        Contact = new OpenApiContact
        {
            Name = "Erickson de Quadros",
            Email = "Ericksondequadros@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/erickson-de-quadros-cz/")
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://www.linkedin.com/in/erickson-de-quadros-cz/"),
        }
    });
    c.EnableAnnotations();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Scheduler API");
    c.RoutePrefix = "taskchedulerAPI";
});

app.UseHangfireDashboard("/hangfireDashBoard", new DashboardOptions
{
    Authorization = HangFireDashBoard.AuthAuthorizationFilters()
});

app.Run();