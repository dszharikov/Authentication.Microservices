using IdentityModule.Web.Application.Services.Account;
using IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.ViewModels;
using MediatR;

namespace IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.Queries;

public class AuthorizeAccountRequest : IRequest<IResult>
{
    public AuthorizeAccountRequest(AuthorizeViewModel model) => Model = model;

    public AuthorizeViewModel Model { get; }
}

/// <summary>
/// Response: Register new account
/// </summary>
public class AuthorizeAccountRequestHandler : IRequestHandler<AuthorizeAccountRequest, IResult>
{
    private readonly IAccountService _accountService;

    public AuthorizeAccountRequestHandler(IAccountService accountService)
        => _accountService = accountService;

    public Task<IResult> Handle(AuthorizeAccountRequest request, CancellationToken cancellationToken)
        => _accountService.AuthorizeAsync(request.Model, cancellationToken);
}
