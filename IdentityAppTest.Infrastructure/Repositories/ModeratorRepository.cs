using IdentityAppTest.Core.Entities.Users;
using IdentityAppTest.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppTest.Infrastructure.Repositories;

public class ModeratorRepository : IRepository<Moderator>
{
    protected readonly UsersContext _dbContext;

    public ModeratorRepository()
    {
        _dbContext = new UsersContext();
    }

    public ModeratorRepository(UsersContext usersContext)
    {
        _dbContext = usersContext;
    }

    public async Task Create(Moderator item)
    {
        await _dbContext.Moderators.AddAsync(item);
    }

    public async Task Delete(Guid guid)
    {
        var obj = await _dbContext.Moderators.FirstOrDefaultAsync(x => x.Id == guid);
        if(obj != null)
            _dbContext.Moderators.Remove(obj);
    }

    public async Task<Moderator> Get(Guid guid)
    {
        return await _dbContext.Moderators.FirstOrDefaultAsync(x => x.Id == guid);
    }

    public IEnumerable<Moderator> GetAll()
    {
        return _dbContext.Moderators;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Moderator item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
