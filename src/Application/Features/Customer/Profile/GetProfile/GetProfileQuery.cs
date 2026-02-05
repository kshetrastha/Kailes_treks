using MediatR;
using TravelCleanArch.Application.Features.Customer.Profile.Models;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Customer.Profile.GetProfile;

public sealed record GetProfileQuery(int UserId) : IRequest<Result<UserProfileDto>>;
