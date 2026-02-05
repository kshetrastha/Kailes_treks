using MediatR;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result>;
