namespace Password;

public class PasswordPublishedDto
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime NotBefore { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Event { get; set; }

    public PasswordPublishedDto(OneTimePassword oneTimePassword, string @event)
    {
        PhoneNumber = oneTimePassword.PhoneNumber;
        Code = oneTimePassword.Code;
        NotBefore = oneTimePassword.NotBefore;
        ExpiresAt = oneTimePassword.ExpiresAt;
        Event = @event;
    }
}