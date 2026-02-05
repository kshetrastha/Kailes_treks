using MediatR;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Auth.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string ResetToken, string NewPassword) : IRequest<Result>;
