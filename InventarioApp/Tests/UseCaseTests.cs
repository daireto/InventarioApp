using InventarioApp.src.Application.Services;
using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;
using InventarioApp.src.Infrastructure.Repositories;
using Xunit;

namespace InventarioApp.Tests;

public class UseCaseTests
{
    private readonly IRepository<Product> _repository = new JsonRepository<Product>("C:\\temp\\test-inventory.json");

    [Fact]
    public void AddProduct_ShouldAddProductToInventory()
    {
        // Arrange
        var inventoryService = new InventoryService(_repository);
        // Act
        var product = new PerishableProduct("1", "Leche", "Leche Colanta", 3500, 100, new DateTime(2025, 9, 15));
        inventoryService.AddProduct(product);
        // Assert
        var allProducts = inventoryService.ListProducts();
        Assert.Contains(product, allProducts);
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateProductDetails()
    {
        // Arrange
        var inventoryService = new InventoryService(_repository);
        var product = new PerishableProduct("2", "Yogurt", "Yogurt Griego", 2500, 50, new DateTime(2025, 10, 1));
        inventoryService.AddProduct(product);
        // Act
        inventoryService.UpdateProduct("2", "Yogurt Natural", "Yogurt Natural Sin Azúcar", 2700);
        // Assert
        var updatedProduct = inventoryService.ListProducts().FirstOrDefault(p => p.Id == "2");
        Assert.NotNull(updatedProduct);
        Assert.Equal("Yogurt Natural", updatedProduct.Name);
        Assert.Equal("Yogurt Natural Sin Azúcar", updatedProduct.Description);
        Assert.Equal(2700, updatedProduct.Price);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProductFromInventory()
    {
        // Arrange
        var inventoryService = new InventoryService(_repository);
        var product = new PerishableProduct("3", "Queso", "Queso Cheddar", 15000, 20, new DateTime(2025, 9, 25));
        inventoryService.AddProduct(product);
        // Act
        inventoryService.DeleteProduct("3");
        // Assert
        var allProducts = inventoryService.ListProducts();
        Assert.DoesNotContain(product, allProducts);
    }

    [Fact]
    public void GetLowStockProductsReport_ShouldReturnCorrectReport()
    {
        // Arrange
        var inventoryService = new InventoryService(_repository);
        var reportService = new ReportService(_repository);
        var product1 = new PerishableProduct("6", "Mantequilla", "Mantequilla Salada", 8000, 3, new DateTime(2025, 11, 15));
        var product2 = new NonPerishableProduct("7", "Detergente", "Detergente Líquido", 12000, 10, "Aseo");
        inventoryService.AddProduct(product1);
        inventoryService.AddProduct(product2);
        // Act
        string report = reportService.GetLowStockProductsReport(5);
        // Assert
        Assert.Contains("Mantequilla", report);
        Assert.DoesNotContain("Huevos", report);
    }

    [Fact]
    public void FindProductsByName_ShouldReturnMatchingProducts()
    {
        // Arrange
        var inventoryService = new InventoryService(_repository);
        var product1 = new PerishableProduct("4", "Pan", "Pan Integral", 5000, 30, new DateTime(2025, 9, 19));
        var product2 = new NonPerishableProduct("5", "Panela", "Panela Orgánica", 6000, 15, "Alimentos");
        inventoryService.AddProduct(product1);
        inventoryService.AddProduct(product2);
        // Act
        var foundProducts = inventoryService.FindProductsByName("Pan");
        // Assert
        Assert.Contains(product1, foundProducts);
        Assert.Contains(product2, foundProducts);
    }
}
