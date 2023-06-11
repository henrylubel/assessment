namespace Assessment.Core.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}