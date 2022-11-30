using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Password;
using Password.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddDbContext<ApplicationDbContext>(config => { config.UseInMemoryDatabase("Password DEMO"); });

var app = builder.Build();

var random = new Random();

app.MapPost("/generate",
    async ([FromServices] ApplicationDbContext context, [FromServices] IMessageBusClient busClient,
        [FromBody] PasswordViewModel viewModel) =>
    {
        var password = new OneTimePassword(viewModel.PhoneNumber, random);

        context.OneTimePasswords.Add(password);

        await context.SaveChangesAsync();

        var passwordPublishedDto = new PasswordPublishedDto(password, "Password_Created");
        busClient.PublishNewPassword(passwordPublishedDto);
        
        return Results.Ok("The password was generated");
    });

app.Run();