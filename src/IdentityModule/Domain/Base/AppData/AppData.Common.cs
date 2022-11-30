namespace IdentityModule.Domain.Base.AppData;

/// <summary>
/// Static data container
/// </summary>
public static partial class AppData
{
    /// <summary>
    /// Current service name
    /// </summary>
    public const string ServiceName = "Microservice Template with OpenIddict";

    /// <summary>
    /// Microservice Template with integrated OpenIddict
    /// for OpenID Connect server and Token Validation
    /// </summary>
    public const string ServiceDescription = "Microservice Template with integrated OpenIddict for OpenID Connect server and Token Validation";

    /// <summary>
    /// "SystemAdministrator"
    /// </summary>
    public const string SystemAdministratorRoleName = "Administrator";

    /// <summary>
    /// "BusinessOwner"
    /// </summary>
    public const string ManagerRoleName = "Manager";
        
    /// <summary>
    /// "User"
    /// </summary>
    public const string UserRoleName = "User";


    /// <summary>
    /// Roles
    /// </summary>
    public static IEnumerable<string> Roles
    {
        get
        {
            yield return SystemAdministratorRoleName;
            yield return ManagerRoleName;
            yield return UserRoleName;
        }
    }
}