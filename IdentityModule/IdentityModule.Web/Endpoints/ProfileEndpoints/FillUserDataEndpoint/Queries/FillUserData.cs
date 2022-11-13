using IdentityModule.Web.Application.Services.Account;
using IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.ViewModels;
using MediatR;

namespace IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.Queries;

public class FillUserDataRequest : IRequest<IResult>
{
    public FillUserDataRequest(FillUserDataViewModel model) => Model = model;

    public FillUserDataViewModel Model { get; }
}

/// <summary>
/// Response: Register new account
/// </summary>
public class FillUserDataRequestHandler : IRequestHandler<FillUserDataRequest, IResult>
{
    private readonly IAccountService _accountService;

    public FillUserDataRequestHandler(IAccountService accountService)
        => _accountService = accountService;

    public Task<IResult> Handle(FillUserDataRequest request, CancellationToken cancellationToken)
        => _accountService.FillUserDataAsync(request.Model, cancellationToken);
}
