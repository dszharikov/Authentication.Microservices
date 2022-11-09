using Microsoft.AspNetCore.Mvc;
using PasswordGeneratorAPI.Data;

namespace PasswordGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;

        public CreationController(ApplicationDbContext context)
        {
            _context = context;
            _random = new Random();
        }
        [HttpGet]
        public async Task<IActionResult> GeneratePassword([FromBody] string phoneNumber)
        {
            try
            {

                var otp = new Password
                {
                    PhoneNumber = phoneNumber,
                    PasswordString = RandomString(4),
                    NotBefore = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddMinutes(5),
                    IsActive = true
                };

                _context.Passwords.Add(otp);

                // todo: send otp to identity server

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
