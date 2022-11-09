using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;

namespace GatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "OtpApi")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly string passwordLink;
        private readonly IHttpClientFactory _httpClientFactory;

        public PasswordController(ConfigurationManager configuration, IHttpClientFactory httpClientFactory)
        {
            passwordLink = configuration["PasswordGenerator"];
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] string phoneNumber)
        {
            var client = _httpClientFactory.CreateClient("passwordgenerator");
            var content = new StringContent(JsonSerializer.Serialize(phoneNumber), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else return BadRequest("There is a server error. Sorry, please try again later.");
        }
    }
}
