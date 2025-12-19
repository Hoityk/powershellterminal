using System.Collections.Generic;

namespace PowerShellTerminal.App.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id); 
        void Add(T entity);
        void Delete(int id);
    }
}