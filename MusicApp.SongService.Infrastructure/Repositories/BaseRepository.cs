using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;

namespace MusicApp.SongService.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
{
    private readonly AppDbContext _appContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext appContext)
    {
        _appContext = appContext;
        _dbSet = _appContext.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
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
