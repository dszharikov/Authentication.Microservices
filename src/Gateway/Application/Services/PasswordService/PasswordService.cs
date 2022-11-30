using System.Text;
using System.Text.Json;
using Gateway.Dtos;

namespace Gateway.Application.Services.PasswordService;

public class PasswordService : IPasswordService
{
    private readonly HttpClient _httpClient;

    public PasswordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IResult> GeneratePassword(PasswordGeneratorDto passwordGeneratorDto)
    {
        var uri = _httpClient.BaseAddress;

        var jsonPhoneNumber = JsonSerializer.Serialize(passwordGeneratorDto);
        
        Console.WriteLine(uri);
        var result = await _httpClient.PostAsync(uri, new StringContent(jsonPhoneNumber, Encoding.UTF8, "application/json"));

        if (result.IsSuccessStatusCode)
        {
            return Results.Ok();
        }

        return Results.Problem("--> Generate Password something happened...");
    }
}