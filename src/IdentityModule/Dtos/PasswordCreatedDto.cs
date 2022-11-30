namespace IdentityModule.Dtos;

public class PasswordCreatedDto
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime NotBefore { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Event { get; set; }
}