using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Company;
using TravelCleanArch.Domain.Entities;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Infrastructure.Persistence.Repositories;

namespace TravelCleanArch.Infrastructure.Services;

public sealed class BlogPostService(AppDbContext dbContext) : GenericRepository<BlogPost>(dbContext), IBlogPostService
{
    public async Task<IReadOnlyList<BlogPost>> ListOrderedAsync(bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking();
        if (publishedOnly) query = query.Where(x => x.IsPublished);

        return await query
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.Ordering)
            .ThenByDescending(x => x.PublishedOnUtc)
            .ThenByDescending(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<BlogPost?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken ct)
    {
        var query = Query().AsNoTracking().Where(x => x.Slug == slug);
        if (publishedOnly) query = query.Where(x => x.IsPublished);
        return query.FirstOrDefaultAsync(ct);
    }
}
