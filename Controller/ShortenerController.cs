using System.Text;
using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;
using Model.Request;
using Model.Response;

namespace Controller;
public static class ShortenerController
{
    public static void ConfigureShortenerController(this WebApplication app){
        app.MapGet("{code}", RedirectShortLink);
        app.MapGet("api/shortener/{code}", GetShortLink);
        app.MapPost("api/shortener", CreateShortLink);
    }
    
    private static async Task<IResult> RedirectShortLink(string code, ICachingService _cache){
        var originalUrl = await _cache.GetAsync(code);
        return Results.Redirect(originalUrl);
    }
    
    private static async Task<IResult> GetShortLink(string code, ICachingService _cache){
        var originalUrl = await _cache.GetAsync(code);
        return Results.Ok(new GetShortLinkResponse(code,originalUrl));
    }

    private static async Task<IResult> CreateShortLink([FromBody] CreateShortLinkRequest request, ICachingService _cache){
        var randomKey = GenerateRandomCode(5);
        randomKey+=request.Alias;
        await _cache.SetAsync(randomKey,request.OriginalUrl);
        return Results.Created($"api/shortener/{randomKey}", new CreateShortlinkResponse(randomKey));
    }

    static Random random = new Random();
    static string GenerateRandomCode(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder code = new StringBuilder();
        
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characters.Length);
            code.Append(characters[index]);
        }
        
        return code.ToString();
    }

}