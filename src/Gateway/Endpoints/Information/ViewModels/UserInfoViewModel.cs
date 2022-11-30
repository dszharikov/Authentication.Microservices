using System.ComponentModel.DataAnnotations;

namespace Gateway.Endpoints.Information.ViewModels;

public class UserInfoViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
}