using TravelCleanArch.Domain.Entities.Master;

namespace TravelCleanArch.Application.Abstractions.Master;

public sealed record DifficultyLevelPagedResult(IReadOnlyCollection<DifficultyLevel> Items, int Page, int PageSize, int TotalCount);
