using IdentityAppTest.Core.Entities.Users;
using IdentityAppTest.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppTest.Infrastructure.Repositories;

public class UserRepository : IRepository<User>
{
    protected readonly UsersContext _dbContext;

    public UserRepository()
    {
        _dbContext = new UsersContext();
    }

    public UserRepository(UsersContext usersContext)
    {
        _dbContext = usersContext;
    }

    public async Task Create(User item)
    {
        await _dbContext.Users.AddAsync(item);
    }

    public async Task Delete(Guid guid)
    {
        var obj = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == guid);
        if(obj != null)
            _dbContext.Users.Remove(obj);
    }

    public async Task<User> Get(Guid guid)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == guid);
    }

    public async Task<User> GetByName(string name)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public IEnumerable<User> GetAll()
    {
        return _dbContext.Users;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Update(User item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
