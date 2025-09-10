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

        static void Main(string[] args)
        {
            InitializeServices();
            ShowMenu();
        }

        static void InitializeServices()
        {
            IRepository<Product> productRepository = new JsonRepository<Product>("C:\\temp\\inventory.json");
            _inventoryService = new InventoryService(productRepository);
            _reportService = new ReportService(productRepository);
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
                Console.WriteLine("7. Exit");
                Console.WriteLine("=======================================");
                Console.Write("Ingrese la opción (1-7): ");

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
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Opción inválida, por favor intente de nuevo");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
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
            Console.Write("Ingrese la opción (1 o 2): ");

            int typeChoice = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("Ingrese el ID del producto: ");
            string id = Console.ReadLine() ?? "";

            Console.Write("Ingrese el nombre del producto: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Ingrese la descripción del producto: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Ingrese el precio del producto: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el stock disponible del producto: ");
            int quantity = int.Parse(Console.ReadLine() ?? "0");

            Product product;

            switch (typeChoice)
            {
                case 1:
                    Console.Write("Ingrese la fecha de expiración (yyyy-MM-dd): ");
                    DateTime expirationDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));
                    product = new PerishableProduct(id, name, description, price, quantity, expirationDate);
                    break;
                case 2:
                    Console.Write("Ingrese la categoría del producto: ");
                    string category = Console.ReadLine() ?? "";
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
            Console.Write("Igrese el ID del producto: ");
            string id = Console.ReadLine() ?? "";

            Console.Write("Ingrese el nuevo stock disponible del producto: ");
            int quantity = int.Parse(Console.ReadLine() ?? "0");

            _inventoryService.UpdateProductStock(id, quantity);
            Console.WriteLine("\nStock actualizado exitosamente!");
        }

        static void DeleteProduct()
        {
            Console.WriteLine("=== Eliminar producto ===");
            Console.Write("Ingrese el ID del producto: ");
            string id = Console.ReadLine() ?? "";
            
            _inventoryService.DeleteProduct(id);
            Console.WriteLine("\nProducto eliminado exitosamente!");
        }

        static void ShowLowStockProducts()
        {
            Console.WriteLine("=== Reporte de stock bajo ===");
            Console.Write("Ingrese el umbral de stock bajo (por defecto 5): ");
            string input = Console.ReadLine() ?? "";

            int threshold = string.IsNullOrWhiteSpace(input) ? 5 : int.Parse(input);
            var report = _reportService.GetLowStockProductsReport(threshold);
            Console.WriteLine(report);
        }

        static void FindProductsByName()
        {
            Console.WriteLine("=== Buscar productos por nombre ===");
            Console.Write("Ingrese el nombre o parte del nombre del producto: ");
            string name = Console.ReadLine() ?? "";

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
    }
}
