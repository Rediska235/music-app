namespace MusicApp.PlaylistService.Application.Repositories;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(T model, CancellationToken cancellationToken);
    void Update(T model);
    void Delete(T model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}