using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Features.Admin.Users.CreateUser;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.API.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
[ApiExplorerSettings(GroupName = "admin")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminUsersController(IMediator mediator) : ControllerBase
{
    public sealed record CreateUserRequest(string Email, string Password, string FullName, string Role);
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var cmd = new CreateUserCommand(request.Email, request.Password, request.FullName, request.Role);
        var result = await mediator.Send(cmd, ct);

        return result.IsSuccess
            ? Ok(new { userId = result.Value })
            : BadRequest(new { error = result.Error });
    }
}
