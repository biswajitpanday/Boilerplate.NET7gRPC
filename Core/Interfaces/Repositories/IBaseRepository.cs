using Core.Entities;
using DotNetCore.Repositories;

namespace Core.Interfaces.Repositories;

public interface IBaseRepository<T> : IRepository<T> where T : BaseEntity
{

}