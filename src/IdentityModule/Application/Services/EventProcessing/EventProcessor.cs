using System.Text.Json;
using AutoMapper;
using Calabonga.UnitOfWork;
using IdentityModule.Domain;
using IdentityModule.Dtos;
using IdentityModule.Infrastructure;

namespace IdentityModule.Application.Services.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PasswordCreated:
                AddPassword(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        System.Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Password_Created":
                System.Console.WriteLine("--> Password Published Event Detected");
                return EventType.PasswordCreated;
            default:
                System.Console.WriteLine("--> Could not determine the event type");
                return EventType.Undertermined;
        }
    }

    private void AddPassword(string passwordCreatedMessage)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordRepository = context.OneTimePasswords;

        var platformPublishedDto = JsonSerializer.Deserialize<PasswordCreatedDto>(passwordCreatedMessage);

        try
        {
            var password = _mapper.Map<OneTimePassword>(platformPublishedDto);

            passwordRepository.Add(password);

            context.SaveChangesAsync();
            
            Console.WriteLine($"--> Password was added to DB: {password.PhoneNumber} | {password.Code}");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
        }
    }
}

enum EventType
{
    PasswordCreated,
    Undertermined
}