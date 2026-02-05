using MediatR;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Application.Features.Admin.Users.CreateUser;

public sealed record CreateUserCommand(string Email, string Password, string FullName, string Role) : IRequest<Result<int>>;
