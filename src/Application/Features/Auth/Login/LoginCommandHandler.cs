using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.Login;

public sealed class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, Result<AuthTokensResponse>>
{
    public Task<Result<AuthTokensResponse>> Handle(LoginCommand request, CancellationToken ct)
        => identityService.LoginAsync(request.Email, request.Password, ct);
}
