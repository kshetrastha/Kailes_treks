using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Identity;

namespace TravelCleanArch.API.Controllers;

[ApiController]
[Route("api/auth")]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class AuthController(IIdentityService identityService) : ControllerBase
{
    public sealed record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await identityService.LoginAsync(request.Email, request.Password, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Unauthorized(new { error = result.Error });
    }
}
