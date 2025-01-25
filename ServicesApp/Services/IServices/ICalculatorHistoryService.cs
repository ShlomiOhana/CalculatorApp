using ServicesApp.Enums;
using ServicesApp.Models;

namespace ServicesApp.Services.IServices
{
    public interface ICalculatorHistoryService
    {
        Task<int> Insert(HistoryEntry historyEntry);
        Task<List<HistoryEntry>> GetHistory(OperationType type);
        Task<HistoryStats> GetHistoryStats(OperationType type);
    }
}
