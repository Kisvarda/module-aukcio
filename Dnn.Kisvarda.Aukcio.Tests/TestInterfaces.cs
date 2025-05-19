using System.Collections.Generic;

public interface IRepository<T>
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
    T GetById(int itemId, int moduleId);
    IEnumerable<T> Get();
}

public interface IDataContext
{
    IRepository<T> GetRepository<T>();
}
