using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Core.Interfaces
{
   public interface IGenericRepository<T> where T:BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity, int userId);
        void Update(T entity, int userId);
        void Delete(T entity);
        void Delete(T entity, int userId);

        Task<IReadOnlyList<T>> StoredProc(string stored, object[] parameters);
        Task<IReadOnlyList<T>> StoredProc(string stored);

    }
}
