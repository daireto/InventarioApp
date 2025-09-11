using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;

namespace InventarioApp.src.Infrastructure.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private List<T> _items;

        public InMemoryRepository()
        {
            _items = [];
        }

        public void Add(T item)
        {
            var itemId = item.Id.ToString();
            if (string.IsNullOrEmpty(itemId))
                throw new ArgumentException("El Id del recurso no puede ser nulo o vacío", nameof(item));

            if (_items.Any(i => i.Id == itemId))
                throw new InvalidOperationException($"Ya existe un recurso con Id {itemId}");

            _items.Add(item);
        }

        public void Update(T item)
        {
            var itemId = item.Id.ToString();
            if (string.IsNullOrEmpty(itemId))
                throw new ArgumentException("El Id del recurso no puede ser nulo o vacío", nameof(item));

            int index = _items.FindIndex(i => i.Id == itemId);
            if (index < 0)
                throw new KeyNotFoundException($"Recurso con ID {itemId} no encontrado");

            _items[index] = item;
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID requerido", nameof(id));

            int index = _items.FindIndex(i => i.Id == id);
            if (index < 0)
                throw new KeyNotFoundException($"Recurso con ID {id} no encontrado");

            _items.RemoveAt(index);
        }

        public T? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID requerido", nameof(id));

            return _items.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }
    }
}
