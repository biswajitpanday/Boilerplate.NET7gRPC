using DotNetCore.EntityFrameworkCore;
using GRPC.NET7.Profile.Core.Entities;
using GRPC.NET7.Profile.Core.Interfaces.Repositories;
using GRPC.NET7.Profile.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace GRPC.NET7.Profile.Repository.Base;

public class BaseRepository<T> : EFRepository<T>, IBaseRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    private readonly IQueryable<T?> _queryable;

    public BaseRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _queryable = _dbContext.QuerySet<T?>().Where(x => !x!.IsDeleted);
    }

    public async Task SaveChangesAsync(bool acceptAllChangesOnSuccess)
        => await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    public void Save() => _dbContext.SaveChanges();

    public async Task RollbackAsync() => await _dbContext.DisposeAsync();
    public void Rollback() => _dbContext.Dispose();
}