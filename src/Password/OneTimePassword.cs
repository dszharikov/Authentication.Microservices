public class OneTimePassword
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime NotBefore { get; set; }
    public DateTime ExpiresAt { get; set; }

    public OneTimePassword(string phoneNumber, Random random, int length = 4, int expiresAfter = 10)
    {
        const string chars = "0123456789";
        PhoneNumber = phoneNumber;
        Code = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        NotBefore = DateTime.Now;
        ExpiresAt = DateTime.Now.AddMinutes(10);
    }

    public OneTimePassword()
    {
        
    }
}