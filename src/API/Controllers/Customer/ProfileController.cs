using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Features.Customer.Profile.GetProfile;
using TravelCleanArch.Domain.Constants;

namespace TravelCleanArch.API.Controllers.Customer;

[ApiController]
[Route("api/customer/profile")]
[ApiExplorerSettings(GroupName = "customer")]
[Authorize(Roles = AppRoles.Customer + "," + AppRoles.Admin)]
public sealed class ProfileController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Unauthorized();

        var result = await mediator.Send(new GetProfileQuery(userId.Value), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }
}
