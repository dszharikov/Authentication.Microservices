﻿using Calabonga.Microservices.Core.Validators;
using Calabonga.OperationResults;
using IdentityModule.Infrastructure;
using IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.ViewModels;
using IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.ViewModels;
using System.Security.Claims;

namespace IdentityModule.Web.Application.Services.Account
{
    /// <summary>
    /// Represent interface for account management
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Returns a collection of the <see cref="ApplicationUser"/> by emails
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersByEmailsAsync(IEnumerable<string> emails);

        /// <summary>
        /// Get User Id from HttpContext
        /// </summary>
        /// <returns></returns>
        Guid GetCurrentUserId();

        /// <summary>
        /// Returns <see cref="ApplicationUser"/> instance after successful registration
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult<IResult>> RegisterAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<IResult> AuthorizeAsync(AuthorizeViewModel model, CancellationToken cancellationToken);
        Task<IResult> FillUserDataAsync(FillUserDataViewModel model, CancellationToken cancellationToken);

        Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Returns User by user identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetByIdAsync(Guid id);

        /// <summary>
        /// Returns ClaimPrincipal by user identity
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetPrincipalByIdAsync(string identifier);

        /// <summary>
        /// Returns ClaimPrincipal by user identity
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetPrincipalByEmailAsync(string email);

        /// <summary>
        /// Returns current user account information or null when user does not logged in
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetCurrentUserAsync();

        /// <summary>
        /// Check roles for current user
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        Task<PermissionValidationResult> IsInRolesAsync(string[] roleNames);

        /// <summary>
        /// Returns all system administrators registered in the system
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
    }
}