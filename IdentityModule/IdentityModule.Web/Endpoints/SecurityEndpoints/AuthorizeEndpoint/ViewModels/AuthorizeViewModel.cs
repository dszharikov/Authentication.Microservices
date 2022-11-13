using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.ViewModels;

/// <summary>
/// Data transfer object for user authorization
/// </summary>
public class AuthorizeViewModel
{
    /// <summary>
    /// PhoneNumber
    /// </summary>
    [Required]
    [StringLength(12, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 11)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    [StringLength(4, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    public string Password { get; set; } = null!;
}