using InventarioApp.src.Application.Services;
using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;
using InventarioApp.src.Infrastructure.Repositories;

namespace InventarioApp.src
{
    class Program
    {
        private static InventoryService _inventoryService;
        private static ReportService _reportService;
        private static CSVExportService _csvExportService;

        static void Main(string[] args)
        {
            InitializeServices();
            ShowMenu();
        }

        static void InitializeServices()
        {
            IRepository<Product> productRepository = new ProductJsonRepository<Product>("C:\\temp\\inventario.json");
            _inventoryService = new InventoryService(productRepository);
            _reportService = new ReportService(productRepository);
            _csvExportService = new CSVExportService();
        }

        static void ShowMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("===== Sistema de inventario de productos =====");
                Console.WriteLine("1. Registrar producto");
                Console.WriteLine("2. Consultar productos");
                Console.WriteLine("3. Actualizar stock de un producto");
                Console.WriteLine("4. Eliminar producto");
                Console.WriteLine("5. Reporte de stock bajo");
                Console.WriteLine("6. Buscar productos por nombre");
                Console.WriteLine("7. Exportar productos a CSV");
                Console.WriteLine("8. Salir");
                Console.WriteLine("=======================================");
                Console.Write("Ingrese la opción (1-8): ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine();

                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddProduct();
                                break;
                            case 2:
                                ListAllProducts();
                                break;
                            case 3:
                                UpdateProductStock();
                                break;
                            case 4:
                                DeleteProduct();
                                break;
                            case 5:
                                ShowLowStockProducts();
                                break;
                            case 6:
                                FindProductsByName();
                                break;
                            case 7:
                                ExportProductsToCSV();
                                break;
                            case 8:
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Opción inválida, por favor intente de nuevo");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n{ex.Message}");
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida, por favor ingresa un número");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Programa finalizado, hasta pronto!");
        }

        static void AddProduct()
        {
            Console.WriteLine("=== Registrar producto ===");
            Console.WriteLine("Seleccione el tipo de producto:");
            Console.WriteLine("1. Perecedero (por defecto)");
            Console.WriteLine("2. No perecedero");
            int typeChoice = ReadInt("Ingrese la opción (1 o 2): ", 1);
            
            string id = ReadLine("Ingrese el ID del producto: ");
            string name = ReadLine("Ingrese el nombre del producto: ");
            string description = ReadLine("Ingrese la descripción del producto: ");
            decimal price = ReadDecimal("Ingrese el precio del producto: ");
            int quantity = ReadInt("Ingrese el stock disponible del producto: ");

            Product product;

            switch (typeChoice)
            {
                case 1:
                    string input = ReadLine("Ingrese la fecha de expiración (aaaa-MM-dd): ");
                    DateTime expirationDate = string.IsNullOrWhiteSpace(input) ? DateTime.Now : DateTime.Parse(input);
                    product = new PerishableProduct(id, name, description, price, quantity, expirationDate);
                    break;
                case 2:
                    string category = ReadLine("Ingrese la categoría del producto: ");
                    product = new NonPerishableProduct(id, name, description, price, quantity, category);
                    break;
                default:
                    Console.WriteLine("\nOpción inválida, debe seleccionar un tipo de producto correcto");
                    return;
            }

            _inventoryService.AddProduct(product);
            Console.WriteLine("\nProducto registrado exitosamente!");
        }

        static void ListAllProducts()
        {
            Console.WriteLine("=== Consultar productos ===");
            var products = _inventoryService.ListProducts();

            if (!products.Any())
            {
                Console.WriteLine("\nNo se encontraron productos.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine("\n" + product.GetDisplayInfo());
            }
        }

        static void UpdateProductStock()
        {
            Console.WriteLine("=== Actualizar stock de un producto ===");
            string id = ReadLine("Ingrese el ID del producto: ");
            int quantity = ReadInt("Ingrese el nuevo stock disponible del producto: ");

            _inventoryService.UpdateProductStock(id, quantity);
            Console.WriteLine("\nStock actualizado exitosamente!");
        }

        static void DeleteProduct()
        {
            Console.WriteLine("=== Eliminar producto ===");
            string id = ReadLine("Ingrese el ID del producto: ");

            _inventoryService.DeleteProduct(id);
            Console.WriteLine("\nProducto eliminado exitosamente!");
        }

        static void ShowLowStockProducts()
        {
            Console.WriteLine("=== Reporte de stock bajo ===");
            int threshold = ReadInt("Ingrese el umbral de stock bajo (por defecto 5): ", 5);
            var report = _reportService.GetLowStockProductsReport(threshold);
            Console.WriteLine(report);
        }

        static void FindProductsByName()
        {
            Console.WriteLine("=== Buscar productos por nombre ===");
            string name = ReadLine("Ingrese el nombre o parte del nombre del producto: ");

            var products = _inventoryService.FindProductsByName(name);
            if (!products.Any())
            {
                Console.WriteLine("\nNo se encontraron productos que coincidan con la búsqueda.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine("\n" + product.GetDisplayInfo());
            }
        }

        static void ExportProductsToCSV()
        {
            Console.WriteLine("=== Exportar productos a CSV ===");
            string filePath = ReadLine("Ingrese la ruta del archivo CSV de salida: ");

            var products = _inventoryService.ListProducts();
            if (!products.Any())
            {
                Console.WriteLine("\nNo se encontraron productos para exportar.");
                return;
            }

            _csvExportService.ExportProducts(products, filePath);
            Console.WriteLine("\nProductos exportados exitosamente!");
        }

        static string ReadLine(string prompt, string defaultValue = "")
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        static int ReadInt(string prompt, int defaultValue = 0)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }
            try
            {
                return int.Parse(input);
            }
            catch
            {
                Console.WriteLine("Entrada inválida, se usará el valor por defecto: " + defaultValue);
                return defaultValue;
            }
        }

        static decimal ReadDecimal(string prompt, decimal defaultValue = 0)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }
            try
            {
                return decimal.Parse(input);
            }
            catch
            {
                Console.WriteLine("Entrada inválida, se usará el valor por defecto: " + defaultValue);
                return defaultValue;
            }
        }
    }
}
