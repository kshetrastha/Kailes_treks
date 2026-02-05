using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Features.Auth.ForgotPassword;
using TravelCleanArch.Application.Features.Auth.Login;
using TravelCleanArch.Application.Features.Auth.Refresh;
using TravelCleanArch.Application.Features.Auth.Register;
using TravelCleanArch.Application.Features.Auth.ResetPassword;

namespace TravelCleanArch.API.Controllers;

[ApiController]
[Route("api/auth")]
[ApiExplorerSettings(GroupName = "customer")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    public sealed record LoginRequest(string Email, string Password);
    public sealed record RegisterRequest(string Email, string Password, string FullName);
    public sealed record RefreshRequest(string RefreshToken);
    public sealed record ForgotPasswordRequest(string Email);
    public sealed record ResetPasswordRequest(string Email, string ResetToken, string NewPassword);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Unauthorized(new { error = result.Error });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterCommand(request.Email, request.Password, request.FullName), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshTokenCommand(request.RefreshToken), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Unauthorized(new { error = result.Error });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new ForgotPasswordCommand(request.Email), ct);

        return result.IsSuccess
            ? Ok(new { message = "If this email exists, reset instructions were generated." })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new ResetPasswordCommand(request.Email, request.ResetToken, request.NewPassword), ct);

        return result.IsSuccess
            ? Ok(new { message = "Password reset successful." })
            : BadRequest(new { error = result.Error });
    }
}
