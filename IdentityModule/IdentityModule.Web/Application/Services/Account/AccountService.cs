using AutoMapper;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.Microservices.Core.Extensions;
using Calabonga.Microservices.Core.Validators;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using IdentityModule.Domain.Base;
using IdentityModule.Infrastructure;
using IdentityModule.Web.Application.Services.PasswordVerificator;
using IdentityModule.Web.Definitions.Identity;
using IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.ViewModels;
using IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace IdentityModule.Web.Application.Services.Account
{
    /// <summary>
    /// Account service
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationUserClaimsPrincipalFactory _claimsFactory;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPasswordVerificator _passwordVerificator;

        public AccountService(
            IUserStore<ApplicationUser> userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<RoleManager<ApplicationRole>> loggerRole,
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<AccountService> logger,
            ILogger<UserManager<ApplicationUser>> loggerUser,
            ApplicationUserClaimsPrincipalFactory claimsFactory,
            IHttpContextAccessor httpContext,
            IPasswordVerificator passwordVerificator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _claimsFactory = claimsFactory;
            _httpContext = httpContext;
            _mapper = mapper;

            // We need to created a custom instance for current service
            // It'll help to use Transaction in the Unit Of Work
            _userManager = new UserManager<ApplicationUser>(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, loggerUser);
            var roleStore = new RoleStore<ApplicationRole, ApplicationDbContext, Guid>(_unitOfWork.DbContext);
            _roleManager = new RoleManager<ApplicationRole>(roleStore, roleValidators, keyNormalizer, errors, loggerRole);
            _passwordVerificator = passwordVerificator;
        }

        /// <inheritdoc />
        public Guid GetCurrentUserId()
        {
            var identity = _httpContext.HttpContext?.User.Identity;
            var identitySub = identity?.GetSubjectId();
            return identitySub?.ToGuid() ?? Guid.Empty;
        }

        /// <summary>
        /// Returns <see cref="ApplicationUser"/> instance after successful registration
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OperationResult<IResult>> RegisterAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<IResult>();

            var user = new ApplicationUser
            {
                UserName = "Anon",
                Email = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true
            };
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            var result = await _userManager.CreateAsync(user);
            const string role = AppData.UserRoleName;

            if (result.Succeeded)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    operation.Exception = new MicroserviceUserNotFoundException();
                    operation.AddError(AppData.Exceptions.UserNotFoundException);
                    operation.Result = Results.NotFound(AppData.Exceptions.UserNotFoundException);
                    return await Task.FromResult(operation);
                }

                await _userManager.AddToRoleAsync(user, role);

                var profile = new ApplicationUserProfile
                {
                    ApplicationUser = user
                };

                var profileRepository = _unitOfWork.GetRepository<ApplicationUserProfile>();

                // Creating list of permittions

                var permissions = new List<AppPermission>
                {
                    new()
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Registration",
                        PolicyName = "Identity:FillUserData",
                        Description = "Access policy for filling user data"
                    }
                };
                profile.Permissions = permissions;


                await profileRepository.InsertAsync(profile, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
                if (_unitOfWork.LastSaveChangesResult.IsOk)
                {
                    var principal = await _claimsFactory.CreateAsync(user);
                    operation.Result = Results.SignIn(principal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    operation.AddSuccess(AppData.Messages.UserSuccessfullyRegistered);
                    _logger.LogInformation(operation.GetMetadataMessages());
                    await transaction.CommitAsync(cancellationToken);
                    _logger.MicroserviceUserRegistration(phoneNumber);
                    return await Task.FromResult(operation);
                }
            }
            var errors = result.Errors.Select(x => $"{x.Code}: {x.Description}");
            operation.AddError(string.Join(", ", errors));
            operation.Result = Results.Problem("The specified user can not be registered.");
            await transaction.RollbackAsync(cancellationToken);
            operation.Exception = _unitOfWork.LastSaveChangesResult.Exception;
            _logger.MicroserviceUserRegistration(phoneNumber, operation.Exception);
            return await Task.FromResult(operation);
        }
        /// <summary>
        /// Returns JWT-token after successful authorization
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResult> AuthorizeAsync(AuthorizeViewModel model, CancellationToken cancellationToken)
        {
            var password = model.Password;
            var phoneNumber = model.PhoneNumber;

            var user = await GetUserByPhoneNumberAsync(phoneNumber, cancellationToken);

            var passwordVerificationResult = await _passwordVerificator.CheckOneTimePassword(phoneNumber, password);

            if (passwordVerificationResult is PasswordVerificationResult.Failed)
            {
                return Results.Problem("The specified user can not be authorized.");
            }

            if (user is null)
            {
                var result = await RegisterAsync(phoneNumber, cancellationToken);

                if (result.Result is null)
                {
                    throw new NullReferenceException($"Registration result is null, {phoneNumber}");
                }

                return result.Result;
            }
            else
            {
                var principal = await GetPrincipalByPhoneNumberAsync(phoneNumber);
                return Results.SignIn(principal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }

        public async Task<IResult> FillUserDataAsync(FillUserDataViewModel model, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            var userId = GetCurrentUserId();
            var user = await _userManager.GetUserAsync(await GetPrincipalByIdAsync(userId.ToString()));

            var profilesRepository = _unitOfWork.GetRepository<ApplicationUserProfile>();
            var userProfile = await profilesRepository
                .GetFirstOrDefaultAsync(
                predicate: profile => profile.ApplicationUser == user,
                include: i => i.Include(x => x.Permissions),
                disableTracking: false);

            /*var permittionRepository = _unitOfWork.GetRepository<AppPermission>();
            var permittions = await permittionRepository
                .GetPagedListAsync(
                predicate: permittion => permittion.ApplicationUserProfile == userProfile,
                disableTracking: false);

            permittionRepository.Delete(permittions);*/

            if (userProfile is null || userProfile.Permissions is null)
            {
                throw new ArgumentNullException($"User profile is null {userId}");
            }

            userProfile.Permissions.Clear();

            userProfile.Permissions.AddRange(
                new List<AppPermission>
                {

                    new()
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "FillUserData",
                        PolicyName = "Contacts:Read",
                        Description = "Access policy for reading contacts"
                    },
                    new()
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "FillUserData",
                        PolicyName = "Contacts:Write",
                        Description = "Access policy for writing contacts"
                    },
                    new()
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "FillUserData",
                        PolicyName = "FirstName:Change",
                        Description = "Access policy for name changing"
                    }
                }
                );

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            await _userManager.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
            if (_unitOfWork.LastSaveChangesResult.IsOk)
            {
                var principal = await _claimsFactory.CreateAsync(user);
                await transaction.CommitAsync(cancellationToken);
                _logger.MicroserviceFillUserData(userId.ToString());
                return Results.SignIn(principal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            _logger.MicroserviceFillUserData(userId.ToString(), _unitOfWork.LastSaveChangesResult.Exception);
            await transaction.RollbackAsync(cancellationToken);
            return Results.Problem("Error: Can not update user information.");
        }

        /// <summary>
        /// Returns ClaimPrincipal by user identity
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> GetPrincipalByIdAsync(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new MicroserviceException();
            }
            var userManager = _userManager;
            var user = await userManager.FindByIdAsync(identifier);
            if (user == null)
            {
                throw new MicroserviceUserNotFoundException();
            }

            var defaultClaims = await _claimsFactory.CreateAsync(user);
            return defaultClaims;
        }

        /// <summary>
        /// Returns ClaimPrincipal by user identity
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> GetPrincipalByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new MicroserviceException();
            }
            var userManager = _userManager;
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new MicroserviceUserNotFoundException();
            }

            var defaultClaims = await _claimsFactory.CreateAsync(user);
            return defaultClaims;
        }

        /// <summary>
        /// Returns ClaimPrincipal by user phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> GetPrincipalByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new MicroserviceException();
            }
            var userManager = _userManager;
            var user = await userManager.FindByEmailAsync(phoneNumber);
            if (user == null)
            {
                throw new MicroserviceUserNotFoundException();
            }

            var defaultClaims = await _claimsFactory.CreateAsync(user);
            return defaultClaims;
        }

        /// <summary>
        /// Returns user by his identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            var userManager = _userManager;
            return userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// Returns current user account information or null when user does not logged in
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userManager = _userManager;
            var userId = GetCurrentUserId().ToString();
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationUser>> GetUsersByEmailsAsync(IEnumerable<string> emails)
        {
            var userManager = _userManager;
            var result = new List<ApplicationUser>();
            foreach (var email in emails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null && !result.Contains(user))
                {
                    result.Add(user);
                }
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Check roles for current user
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        public async Task<PermissionValidationResult> IsInRolesAsync(string[] roleNames)
        {
            var userManager = _userManager;
            var userId = GetCurrentUserId().ToString();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var resultUserNotFound = new PermissionValidationResult();
                resultUserNotFound.AddError(AppData.Exceptions.UnauthorizedException);
                return await Task.FromResult(resultUserNotFound);
            }
            foreach (var roleName in roleNames)
            {
                var ok = await userManager.IsInRoleAsync(user, roleName);
                if (ok)
                {
                    return new PermissionValidationResult();
                }
            }

            var result = new PermissionValidationResult();
            result.AddError(AppData.Exceptions.UnauthorizedException);
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            var userManager = _userManager;
            return await userManager.GetUsersInRoleAsync(roleName);
        }

        #region privates

        private async Task AddClaimsToUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string role)
        {
            await userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Name, user.UserName));
            await userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Email, user.Email));
            await userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Role, role));
        }

        public async Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var userManager = _userManager;

            var user = await userManager.Users.FirstOrDefaultAsync(user => user.PhoneNumberConfirmed && user.PhoneNumber == phoneNumber, cancellationToken);

            return user;
        }


        #endregion
    }
}