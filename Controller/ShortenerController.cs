using Core;
using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;
using Model.Request;
using Model.Response;

namespace Controller;
public static class ShortenerController
{
    public static void ConfigureShortenerController(this WebApplication app)
    {
        app.MapGet("{code}", RedirectShortLink);
        app.MapGet("api/shortener/{code}", GetShortLink);
        app.MapPost("api/shortener", CreateShortLink);
    }

    private static async Task<IResult> RedirectShortLink(string code, ICachingService _cache)
    {
        try
        {
            var originalUrl = await _cache.GetAsync(code.ToUpper());
            
            if(string.IsNullOrEmpty(originalUrl)) 
                return Results.BadRequest("Link não encontrado");
            
            return Results.Redirect(originalUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao obter o valor do cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao redirecionar o link.");
        }
    }

    private static async Task<IResult> GetShortLink(string code, ICachingService _cache)
    {
        try
        {
            var originalUrl = await _cache.GetAsync(code.ToUpper());
            
            if(string.IsNullOrEmpty(originalUrl)) 
                return Results.BadRequest("Link não encontrado");

            return Results.Ok(new GetShortLinkResponse(code.ToUpper(), originalUrl));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao obter o valor do cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao recuperar o link.");
        }
    }

    private static async Task<IResult> CreateShortLink([FromBody] CreateShortLinkRequest request, ICachingService _cache)
    {
        try
        {
            var randomKey = ShortenerExtensions.GenerateRandomCode(5);
            randomKey =  string.IsNullOrEmpty(request.Alias) 
                ? randomKey 
                : $"{request.Alias.ToUpper()}-{randomKey}";

            await _cache.SetAsync(randomKey, request.OriginalUrl);
            return Results.Created($"api/shortener/{randomKey}", new CreateShortlinkResponse(randomKey));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao gravar valor no cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao criar o link encurtado.");
        }
    }
}