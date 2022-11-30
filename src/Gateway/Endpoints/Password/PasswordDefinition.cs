using Calabonga.AspNetCore.AppDefinitions;
using Gateway.Application.Services.PasswordService;
using Gateway.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Endpoints.Password;

public class PasswordDefinition : AppDefinition
{
    private IPasswordService? _passwordService;

    public override void ConfigureApplication(WebApplication app)
    {
        app.MapPost("/password", GeneratePassword).ExcludeFromDescription();
        _passwordService = app.Services.GetService<IPasswordService>();

        Console.WriteLine("--> Password Map Post Detected");
    }

    private async Task<IResult> GeneratePassword([FromBody] PasswordGeneratorDto passwordGeneratorDto)
    {
        if (_passwordService is null)
        {
            throw new Exception("--> PasswordService is null");
        }
        
        return await _passwordService.GeneratePassword(passwordGeneratorDto);
    }
}