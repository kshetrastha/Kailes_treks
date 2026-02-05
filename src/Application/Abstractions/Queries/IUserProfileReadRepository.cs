using TravelCleanArch.Application.Features.Customer.Profile.Models;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Abstractions.Queries;

public interface IUserProfileReadRepository
{
    Task<Result<UserProfileDto>> GetProfileAsync(int userId, CancellationToken ct);
}
