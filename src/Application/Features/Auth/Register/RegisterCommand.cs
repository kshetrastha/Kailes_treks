using MediatR;
using TravelCleanArch.Application.Abstractions.Identity;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.Register;

public sealed record RegisterCommand(string Email, string Password, string FullName) : IRequest<Result<AuthTokensResponse>>;
