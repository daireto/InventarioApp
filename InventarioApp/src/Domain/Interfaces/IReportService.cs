namespace InventarioApp.src.Domain.Interfaces
{
    public interface IReportService
    {
        string GetLowStockProductsReport(int threshold = 5);
    }
}
