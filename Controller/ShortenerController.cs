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

            if (string.IsNullOrEmpty(originalUrl))
                return Results.BadRequest("Link não encontrado");

            return Results.Redirect(originalUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao obter o valor do cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao redirecionar o link! Tente novamente mais tarde.");
        }
    }

    private static async Task<IResult> GetShortLink(string code, ICachingService _cache)
    {
        try
        {
            var originalUrl = await _cache.GetAsync(code.ToUpper());

            if (string.IsNullOrEmpty(originalUrl))
                return Results.BadRequest("Link não encontrado");

            return Results.Ok(new GetShortLinkResponse(code.ToUpper(), originalUrl));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao obter o valor do cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao recuperar o link! Tente novamente mais tarde.");
        }
    }

    private static async Task<IResult> CreateShortLink([FromBody] CreateShortLinkRequest request, ICachingService _cache, IConfiguration _configuration)
    {
        try
        {
            var key = request.Alias.ToUpper();

            if (!string.IsNullOrEmpty(key))
            {
                var originalUrl = await _cache.GetAsync(key);

                if (!string.IsNullOrEmpty(originalUrl))
                    return Results.BadRequest("Alias já utilizado!");
            }
            else
            {
                key = ShortenerExtensions.GenerateRandomCode(int.Parse(_configuration["RandomCodeLenght"] ?? "5"));
            }

            await _cache.SetAsync(key, request.OriginalUrl);
            
            return Results.Created(
                $"api/shortener/{key}", 
                new CreateShortlinkResponse(key, DateTime.UtcNow.AddHours(int.Parse(_configuration["ExpirationDate"] ?? "24"))));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção ao gravar valor no cache: {ex.Message}");
            return Results.BadRequest("Ocorreu um erro ao criar o link encurtado! Tente novamente mais tarde.");
        }
    }
}