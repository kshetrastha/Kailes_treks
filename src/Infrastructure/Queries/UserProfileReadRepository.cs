using Dapper;
using TravelCleanArch.Application.Abstractions.Queries;
using TravelCleanArch.Application.Features.Customer.Profile.Models;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.SharedKernel.Results;

namespace TravelCleanArch.Infrastructure.Queries;

public sealed class UserProfileReadRepository(IDbConnectionFactory factory) : IUserProfileReadRepository
{
    public async Task<Result<UserProfileDto>> GetProfileAsync(int userId, CancellationToken ct)
    {
        const string sql = @"
        SELECT ""Id""       AS ""UserId"",
               ""Email""    AS ""Email"",
               ""FullName"" AS ""FullName""
        FROM ""AspNetUsers""
        WHERE ""Id"" = @UserId;";

        using var conn = factory.CreateConnection();
        var dto = await conn.QuerySingleOrDefaultAsync<UserProfileDto>(
            new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct));

        return dto is null
            ? Result<UserProfileDto>.Failure("profile.not_found", "Profile not found.")
            : Result<UserProfileDto>.Success(dto);
    }
}
