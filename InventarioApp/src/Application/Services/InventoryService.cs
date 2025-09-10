using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;

namespace InventarioApp.src.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<Product> _repository;

        public InventoryService(IRepository<Product> productRepository)
        {
            _repository = productRepository;
        }

        public void AddProduct(Product product)
        {
            _repository.Add(product);
        }

        public void UpdateProduct(string id, string name, string description, decimal price)
        {
            var product = _repository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");
            
            product.Rename(name);
            product.UpdateDescription(description);
            product.UpdatePrice(price);

            _repository.Update(product);
        }

        public void UpdateProductStock(string id, int newQuantity)
        {
            var product = _repository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            if (newQuantity > product.Quantity)
                product.IncreaseStock(newQuantity - product.Quantity);
            else if (newQuantity < product.Quantity)
                product.DecreaseStock(product.Quantity - newQuantity);

            _repository.Update(product);
        }

        public void DeleteProduct(string id)
        {
            _repository.Remove(id);
        }

        public IEnumerable<Product> ListProducts()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Product> FindProductsByName(string name)
        {
            return _repository.GetAll().Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
