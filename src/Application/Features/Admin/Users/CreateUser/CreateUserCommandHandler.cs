using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Admin.Users.CreateUser;

public sealed class CreateUserCommandHandler(IIdentityService identityService) : IRequestHandler<CreateUserCommand, Result<int>>
{
    public Task<Result<int>> Handle(CreateUserCommand request, CancellationToken ct)
        => identityService.CreateUserAsync(new CreateUserRequest(request.Email, request.Password, request.FullName, request.Role), ct);
}
