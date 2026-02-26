using System.Linq.Expressions;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Application.Abstractions.Persistence;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Query();
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Remove(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}
