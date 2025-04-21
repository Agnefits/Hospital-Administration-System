namespace Hospital_Administration_System.Repository;

public interface ILogRepository : IRepository<Log>
{
    Task<IEnumerable<Log>> GetRecentLogsAsync(int count);
    Task<IEnumerable<Log>> GetFilteredLogsAsync(DateTime? startDate, DateTime? endDate, string? search);
}