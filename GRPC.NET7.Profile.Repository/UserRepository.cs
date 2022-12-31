using GRPC.NET7.Profile.Core.Entities;
using GRPC.NET7.Profile.Core.Interfaces.Repositories;
using GRPC.NET7.Profile.Repository.Base;
using GRPC.NET7.Profile.Repository.DatabaseContext;

namespace GRPC.NET7.Profile.Repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}