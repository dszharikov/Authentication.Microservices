using System.ComponentModel.DataAnnotations;

namespace IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.ViewModels;

/// <summary>
/// Data transfer object for user extra data
/// </summary>
public class FillUserDataViewModel
{
    /// <summary>
    /// FirstName
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
}