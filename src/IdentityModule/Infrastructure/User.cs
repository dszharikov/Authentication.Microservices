using System.ComponentModel.DataAnnotations.Schema;
using IdentityModule.Domain.Base;

namespace IdentityModule.Infrastructure;

/// <summary>
/// Default user for application.
/// Add profile data for application users by adding properties to the User class
/// </summary>
public class User : Identity
{
    public string PhoneNumber { get; set; }
    /// <summary>
    /// Profile identity
    /// </summary>
    public Guid? UserProfileId { get; set; }

    /// <summary>
    /// User Profile
    /// </summary>
    public virtual UserProfile? UserProfile { get; set; }
}