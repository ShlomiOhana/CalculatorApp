using ServicesApp.DAL;
using ServicesApp.Enums;
using ServicesApp.Models;
using ServicesApp.Services.IServices;

namespace ServicesApp.Services
{
    public class CalculatorHistoryService : ICalculatorHistoryService
    {
        private DatabaseService _dbService;

        public CalculatorHistoryService(DatabaseService service)
        {
            _dbService = service;
        }

        public async Task<List<HistoryEntry>> GetHistory(OperationType type)
        {
            return await _dbService.GetHistoryAsync(type);
        }

        public async Task<HistoryStats> GetHistoryStats(OperationType type)
        {
            HistoryStats result = await _dbService.GetHistoryStatsAsync(type);
            result.CountInLast30Days = await _dbService.GetHistoryLast30DaysAsync(type);
            return result;
        }

        public async Task<int> Insert(HistoryEntry historyEntry)
        {
            return await _dbService.InsertAsync(historyEntry);
        }
    }
}
