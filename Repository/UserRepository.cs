using Core.Entities;
using Core.Interfaces.Repositories;
using Repository.Base;
using Repository.DatabaseContext;

namespace Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}