using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProtoCore.NET.Core.Entities;
using ProtoCore.NET.Core.Interfaces.Repositories;
using ProtoCore.NET.Repository.DatabaseContext;

namespace ProtoCore.NET.Repository.Base;

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