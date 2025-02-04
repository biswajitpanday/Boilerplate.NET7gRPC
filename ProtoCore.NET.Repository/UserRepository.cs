using ProtoCore.NET.Core.Entities;
using ProtoCore.NET.Core.Interfaces.Repositories;
using ProtoCore.NET.Repository.Base;
using ProtoCore.NET.Repository.DatabaseContext;

namespace ProtoCore.NET.Repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}