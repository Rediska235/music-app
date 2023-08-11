using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.SongService.Infrastructure.Extensions;

namespace MusicApp.SongService.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
{
    protected AppDbContext _appContext;
    private readonly DbSet<T> _dbSet;
    protected IDistributedCache _cache;

    public BaseRepository(AppDbContext appContext, IDistributedCache cache)
    {
        _appContext = appContext;
        _dbSet = _appContext.Set<T>();
        _cache = cache;
    }

    public virtual async Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _cache.GetEntityAsync<T>(id.ToString(), cancellationToken);
        if (entity != null)
        {
            return entity;
        }

        entity = await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (entity != null)
        {
            await _cache.SetEntityAsync(id.ToString(), entity, cancellationToken);
        }

        return entity;
    }

    public virtual async Task CreateAsync(T model, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(model, cancellationToken);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Update(T model)
    {
        _dbSet.Update(model);
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _appContext.SaveChangesAsync(cancellationToken);
    }
}
