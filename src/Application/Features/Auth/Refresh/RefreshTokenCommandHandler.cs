using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.Refresh;

public sealed class RefreshTokenCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, Result<AuthTokensResponse>>
{
    public Task<Result<AuthTokensResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
        => identityService.RefreshAsync(request.RefreshToken, ct);
}
