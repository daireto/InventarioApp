using InventarioApp.src.Domain.Entities;

namespace InventarioApp.src.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T item);
        void Update(T item);
        void Remove(string id);
        T? GetById(string id);
        IEnumerable<T> GetAll();
    }
}
