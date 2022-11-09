namespace PasswordGeneratorAPI.Data;

public class Password
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime NotBefore { get; set; }
    public string PasswordString { get; set; }
    public bool IsActive { get; set; }
}
