using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity>(AppDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    private DbSet<TEntity> Set => dbContext.Set<TEntity>();

    public IQueryable<TEntity> Query() => Set;

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(entity => entity.Id == id, ct);

    public Task AddAsync(TEntity entity, CancellationToken ct = default)
        => Set.AddAsync(entity, ct).AsTask();

    public void Remove(TEntity entity) => Set.Remove(entity);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Set.AnyAsync(predicate, ct);
}
