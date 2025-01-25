using Microsoft.Data.SqlClient;
using ServicesApp.Enums;
using ServicesApp.Models;
using System.Data;

namespace ServicesApp.DAL
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = "Server=GALCON-SHLOMIO\\SQLEXPRESS;Database=CalculatorHistory;Trusted_Connection=True;TrustServerCertificate=True";
        }

        public async Task<List<HistoryEntry>> GetHistoryAsync(OperationType type)
        {
            var results = new List<HistoryEntry>();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("GetHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operation", (byte)type);

                    conn.Open();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new HistoryEntry
                            {
                                Date = DateTime.Parse(reader["InsertDate"].ToString()),
                                Field1 = Math.Round(Convert.ToDouble(reader["Field1"]), 2),
                                Field2 = Math.Round(Convert.ToDouble(reader["Field2"]), 2),
                                Operation = Convert.ToByte(reader["Operation"]),
                                Result = Math.Round(Convert.ToDouble(reader["Result"]), 2),
                            });
                        }
                    }
                }
            }
            return results;
        }

        public async Task<HistoryStats> GetHistoryStatsAsync(OperationType type)
        {
            var result = new HistoryStats();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("GetHistoryStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operation", (byte)type);

                    conn.Open();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.MaxResult = Math.Round(Convert.ToDouble(reader["MaxResult"]), 2);
                            result.MinResult = Math.Round(Convert.ToDouble(reader["MinResult"]), 2);
                            result.AvgResult = Math.Round(Convert.ToDouble(reader["AvgResult"]), 2);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> GetHistoryLast30DaysAsync(OperationType type)
        {
            int count = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("GetLastMonthSearches", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operation", (byte)type);

                    conn.Open();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            count = Convert.ToInt32(reader["Count"]);
                        }
                    }
                }
            }
            return count;
        }

        public async Task<int> InsertAsync(HistoryEntry entry)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass parameters to the stored procedure
                    cmd.Parameters.AddWithValue("@Date", entry.Date);
                    cmd.Parameters.AddWithValue("@Field1", entry.Field1);
                    cmd.Parameters.AddWithValue("@Field2", entry.Field2);
                    cmd.Parameters.AddWithValue("@Operation", entry.Operation);
                    cmd.Parameters.AddWithValue("@Result", entry.Result);

                    conn.Open();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
