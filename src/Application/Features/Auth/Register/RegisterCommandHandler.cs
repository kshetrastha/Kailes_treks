using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterCommand, Result<AuthTokensResponse>>
{
    public Task<Result<AuthTokensResponse>> Handle(RegisterCommand request, CancellationToken ct)
        => identityService.RegisterAsync(request.Email, request.Password, request.FullName, ct);
}
