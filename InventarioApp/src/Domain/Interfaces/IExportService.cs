using InventarioApp.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.src.Domain.Interfaces
{
    public interface IExportService
    {
        void ExportProducts(IEnumerable<Product> products, string filePath);
    }
}
