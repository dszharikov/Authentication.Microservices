namespace IdentityModule.Application.Services.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}