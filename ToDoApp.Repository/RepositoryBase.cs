using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Contracts;
using ToDoApp.Entities;

namespace ToDoApp.Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private RepositoryDbContext RepositoryDbContext { get; set; }

    protected RepositoryBase(RepositoryDbContext repositoryDbContext)
    {
        RepositoryDbContext = repositoryDbContext;
    }
    
    public IQueryable<T> FindAll()
    {
        return RepositoryDbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return RepositoryDbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    {
        RepositoryDbContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        RepositoryDbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        RepositoryDbContext.Set<T>().Remove(entity);
    }
}