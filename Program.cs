using Controller;
using Infrastructure.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICachingService, CachingService>();

builder.Services.AddStackExchangeRedisCache(o => {
    o.InstanceName = "instance";
    o.Configuration = "172.17.0.3:6379";
});

builder.Services.AddControllers();

var app = builder.Build();

app.ConfigureShortenerController();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
