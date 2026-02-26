using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthTokensResponse>>;
