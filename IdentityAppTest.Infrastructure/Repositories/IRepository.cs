
namespace IdentityAppTest.Infrastructure.Repositories;

public interface IRepository<T> 
    where T : class
{
    IEnumerable<T> GetAll();
    Task<T> Get(Guid guid);
    Task Create(T item);
    void Update(T item);
    Task Delete(Guid guid);
    Task Save();
}
