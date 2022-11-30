using IdentityModule.Dtos;

namespace IdentityModule.AsyncDataServices.MessageBusClients;

public interface IMessageBusClient
{
    void PublishNewUser(PublishUserDto userPublishDto);
}