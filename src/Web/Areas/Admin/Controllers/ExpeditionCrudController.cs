using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Enumerations;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("admin/api/expedition-crud")]
public sealed class ExpeditionCrudController(IExpeditionService service, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("dropdowns")]
    public IActionResult GetDropdowns()
        => Ok(new ExpeditionDropdownsDto(
            ToOptions<DifficultyLevel>(),
            ToOptions<Country>(),
            ToOptions<TravelStatus>()));

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] TravelStatus? status,
        [FromQuery] Country? country,
        [FromQuery] bool? featured,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 200);

        var result = await service.ListAsync(
            search,
            status?.ToString(),
            country?.ToString(),
            featured,
            page,
            pageSize,
            ct);

        return Ok(result.Items.Select(Map).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(Map(item));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpeditionCrudRequest request, CancellationToken ct)
    {
        var id = await service.CreateAsync(ToUpsert(request), currentUser.UserId, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpeditionCrudRequest request, CancellationToken ct)
        => await service.UpdateAsync(id, ToUpsert(request), currentUser.UserId, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();

    private static ExpeditionUpsertDto ToUpsert(ExpeditionCrudRequest request)
        => new(
            request.Name,
            request.Slug,
            request.ShortDescription,
            request.Destination,
            request.Region,
            request.DurationDays,
            request.MaxAltitudeMeters,
            request.DifficultyLevel.ToString(),
            null,
            null,
            null,
            null,
            null,
            null,
            request.MinGroupSize,
            request.MaxGroupSize,
            request.Price,
            null,
            null,
            null,
            null,
            request.TravelStatus.ToString(),
            request.Featured,
            request.Ordering,
            null,
            false,
            null,
            false,
            false,
            null,
            request.ExpeditionTypeId,
            [],
            [],
            [],
            [],
            request.Country.ToString(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            request.DifficultyLevel.ToString(),
            [],
            [],
            [],
            [],
            [],
            [],
            []);

    private static ExpeditionCrudResponse Map(ExpeditionDetailsDto item)
        => new(
            item.Id,
            item.Name,
            item.Slug,
            item.ShortDescription,
            item.Destination,
            item.Region,
            item.DurationDays,
            item.MaxAltitudeMeters,
            ParseEnum(item.DifficultyLevel, DifficultyLevel.Moderate),
            ParseEnum(item.OverviewCountry, Country.Nepal),
            ParseEnum(item.Status, TravelStatus.Draft),
            item.MinGroupSize,
            item.MaxGroupSize,
            item.Price,
            item.Featured,
            item.Ordering,
            item.ExpeditionTypeId,
            item.ExpeditionTypeTitle);

    private static ExpeditionCrudResponse Map(ExpeditionListItemDto item)
        => new(
            item.Id,
            item.Name,
            item.Slug,
            item.ShortDescription,
            item.Destination,
            null,
            item.DurationDays,
            0,
            ParseEnum(item.DifficultyLevel, DifficultyLevel.Moderate),
            ParseEnum(item.Country, Country.Nepal),
            ParseEnum(item.Status, TravelStatus.Draft),
            0,
            0,
            0,
            item.Featured,
            item.Ordering,
            item.ExpeditionTypeId,
            item.ExpeditionTypeTitle);

    private static IReadOnlyCollection<DropdownOptionDto> ToOptions<TEnum>() where TEnum : struct, Enum
        => Enum.GetValues<TEnum>()
            .Select(value => new DropdownOptionDto(value.ToString(), value.ToString()))
            .ToList();

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct, Enum
        => Enum.TryParse<TEnum>(value, true, out var parsed) ? parsed : fallback;
}
