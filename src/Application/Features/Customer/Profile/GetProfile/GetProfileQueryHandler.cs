using MediatR;
using TravelCleanArch.Application.Abstractions.Queries;
using TravelCleanArch.Application.Features.Customer.Profile.Models;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Customer.Profile.GetProfile;

public sealed class GetProfileQueryHandler(IUserProfileReadRepository repo) : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
{
    public Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken ct)
        => repo.GetProfileAsync(request.UserId, ct);
}
