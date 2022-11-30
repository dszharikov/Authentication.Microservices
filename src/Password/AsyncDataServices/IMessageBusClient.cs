namespace Password.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPassword(PasswordPublishedDto passwordPublishedDto);
}