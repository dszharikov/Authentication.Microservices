namespace IdentityModule.Web.Application
{
    /// <summary>
    /// The number identifiers for events in the microservices
    /// </summary>
    internal static class EventNumbers
    {
        internal static readonly EventId UserRegistrationId = new EventId(9001, nameof(UserRegistrationId));
        internal static readonly EventId PostItemId = new EventId(9002, nameof(PostItemId));
    }

    /// <summary>
    /// Event logging as ILogger extension.
    /// Please see the video as the manual https://youtu.be/09EVKgHgwnM
    /// </summary>
    internal static class LoggerExtensions
    {
        #region UserRegistration

        /// <summary>
        /// EventItem register action event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="exception"></param>
        internal static void MicroserviceUserRegistration(this ILogger source, string phoneNumber, Exception? exception = null)
        {
            switch (exception)
            {
                case null:
                    UserRegistrationExecute(source, phoneNumber, exception);
                    break;
                default:
                    UserRegistrationFailedExecute(source, phoneNumber, exception);
                    break;
            }
        }

        private static readonly Action<Microsoft.Extensions.Logging.ILogger, string, Exception?> UserRegistrationExecute =
            LoggerMessage.Define<string>(LogLevel.Information, EventNumbers.UserRegistrationId,
                "User {userName} successfully registred");

        private static readonly Action<ILogger, string, Exception?> UserRegistrationFailedExecute =
            LoggerMessage.Define<string>(LogLevel.Error, EventNumbers.UserRegistrationId,
                "User {userName} registred failed");

        #endregion

        #region FillUserData

        internal static void MicroserviceFillUserData(this ILogger source, string entityName, Exception? exception = null)
        {
            switch (exception)
            {
                case null:
                    FillUserDataExecute(source, entityName, null);
                    break;

                default:
                    FillUserDataFailedExecute(source, entityName, exception);
                    break;
            }
        }

        private static readonly Action<ILogger, string, Exception?> FillUserDataExecute =
            LoggerMessage.Define<string>(LogLevel.Information, EventNumbers.PostItemId,
                "The {entityName} user info successfully updated");

        private static readonly Action<ILogger, string, Exception?> FillUserDataFailedExecute =
            LoggerMessage.Define<string>(LogLevel.Error, EventNumbers.PostItemId,
                "The {entityName} updating failed");

        #endregion
    }
}