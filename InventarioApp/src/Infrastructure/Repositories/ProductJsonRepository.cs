using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;
using System.Text.Json;

namespace InventarioApp.src.Infrastructure.Repositories
{
    public class ProductJsonRepository<T> : IRepository<T> where T : Product
    {
        private readonly string _filePath;
        private readonly List<T> _items;
        private JsonSerializerOptions _jsonSerializerOptions;

        public IReadOnlyCollection<T> Items => _items.AsReadOnly();

        public ProductJsonRepository(string filePath)
        {
            _filePath = filePath;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            };
            _jsonSerializerOptions.Converters.Add(new ProductJsonConverter());

            _items = Load();
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
            {
                return [];
            }

            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return [];
            }

            var loadedItems = JsonSerializer.Deserialize<List<Product>>(jsonString, _jsonSerializerOptions);
            return loadedItems?.Cast<T>().ToList() ?? [];
        }

        public void SaveChanges()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string jsonString = JsonSerializer.Serialize(_items, _jsonSerializerOptions);
            File.WriteAllText(_filePath, jsonString);
        }

        public void Add(T item)
        {
            var itemId = item.Id.ToString();
            if (string.IsNullOrEmpty(itemId))
                throw new ArgumentException("El Id del recurso no puede ser nulo o vacío", nameof(item));

            if (_items.Any(i => i.Id == itemId))
                throw new InvalidOperationException($"Ya existe un recurso con Id {itemId}");

            _items.Add(item);
            SaveChanges();
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
            SaveChanges();
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID requerido", nameof(id));

            int index = _items.FindIndex(i => i.Id == id);
            if (index < 0)
                throw new KeyNotFoundException($"Recurso con ID {id} no encontrado");

            _items.RemoveAt(index);
            SaveChanges();
        }

        public T? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID requerido", nameof(id));

            return _items.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items;
        }
    }
}
