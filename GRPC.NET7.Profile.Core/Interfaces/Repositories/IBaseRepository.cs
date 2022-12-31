using DotNetCore.Repositories;
using GRPC.NET7.Profile.Core.Entities;

namespace GRPC.NET7.Profile.Core.Interfaces.Repositories;

public interface IBaseRepository<T> : IRepository<T> where T : BaseEntity
{
    /// <param name="acceptAllChangesOnSuccess">
    ///     Indicates whether <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after
    ///     the changes have been sent successfully to the database.
    /// </param>
    Task SaveChangesAsync(bool acceptAllChangesOnSuccess);
    Task SaveChangesAsync();
    void Save();
}