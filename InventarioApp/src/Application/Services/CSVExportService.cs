using InventarioApp.src.Domain.Entities;
using InventarioApp.src.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.src.Application.Services
{
    public class CSVExportService : IExportService
    {
        public void ExportProducts(IEnumerable<Product> products, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Ruta de archivo requerida", nameof(filePath));

            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Nombre,Descripción,Precio,Cantidad,Tipo");

            foreach (var product in products)
            {
                csvContent.AppendLine(product.GetCSVLine());
            }

            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
