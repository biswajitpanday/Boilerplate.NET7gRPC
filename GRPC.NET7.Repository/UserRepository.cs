using GRPC.NET7.Core.Entities;
using GRPC.NET7.Core.Interfaces.Repositories;
using GRPC.NET7.Repository.Base;
using GRPC.NET7.Repository.DatabaseContext;

namespace GRPC.NET7.Repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}