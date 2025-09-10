using InventarioApp.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.src.Domain.Interfaces
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        void UpdateProduct(string id, string name, string description, decimal price);
        void UpdateProductStock(string id, int newQuantity);
        void DeleteProduct(string id);
        IEnumerable<Product> ListProducts();
        IEnumerable<Product> FindProductsByName(string name);
    }
}
