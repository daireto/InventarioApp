using InventarioApp.src.Domain.Interfaces;
using InventarioApp.src.Domain.Entities;

namespace InventarioApp.src.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Product> _repository;

        public ReportService(IRepository<Product> productRepository)
        {
            _repository = productRepository;
        }

        public string GetLowStockProductsReport(int threshold = 5)
        {
            var allProducts = _repository.GetAll();
            var lowStockProducts = allProducts.Where(p => p.Quantity < threshold);
            if (!lowStockProducts.Any())
            {
                return "\nNo hay productos con stock menor a " + threshold;
            }

            string report = $"\nProductos con stock menor a {threshold}:\n\n";
            foreach (var product in lowStockProducts)
            {
                report += $" - {product.GetNameAndStock()}\n";
            }
            return report;
        }
    }
}
