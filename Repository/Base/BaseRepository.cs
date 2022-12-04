using Core.Entities;
using Core.Interfaces.Repositories;
using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository.Base;

public class BaseRepository<T> : EFRepository<T>, IBaseRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IQueryable<T?> _queryable;


    public BaseRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _queryable = _context.QuerySet<T?>().Where(x => !x!.IsDeleted);
    }

    public async Task SaveChangesAsync(bool acceptAllChangesOnSuccess) 
        => await _context.SaveChangesAsync(acceptAllChangesOnSuccess);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Save() => _context.SaveChanges();
}