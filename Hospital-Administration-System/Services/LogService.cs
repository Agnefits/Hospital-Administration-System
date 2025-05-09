
namespace Hospital_Administration_System.Services;


public class LogService : GenericRepository<Log>,  ILogRepository
{
    public LogService(ApplicationDbContext context):base(context)
    {

    }

    public async Task<IEnumerable<Log>> GetAllAsync()
    {
        return await _context.Logs.OrderByDescending(l => l.Timestamp).ToListAsync();
    }

    public async Task<Log> GetByIdAsync(int id)
    {
        return await _context.Logs.FindAsync(id);
    }

    public async Task AddAsync(Log entity)
    {
        await _context.Logs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<Log>> GetFilteredLogsAsync(DateTime? startDate, DateTime? endDate, string? search)
    {
        var logs = _context.Logs.Include(l => l.User).AsQueryable();

        if (startDate.HasValue)
        {
            logs = logs.Where(l => l.Timestamp >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            logs = logs.Where(l => l.Timestamp <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(search))
        {
            logs = logs.Where(l =>
                l.User.UserName.Contains(search) ||
                l.Action.Contains(search) ||
                l.TableName.Contains(search) ||
                l.IPAddress.Contains(search) ||
                l.DeviceInfo.Contains(search) ||
                l.AdditionalData.Contains(search)
            );
        }

        return await logs.OrderByDescending(l => l.Timestamp).ToListAsync();
    }


    public async Task UpdateAsync(Log entity)
    {
        _context.Logs.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Log entity)
    {
        _context.Logs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Log>> GetRecentLogsAsync(int count)
    {
        return await _context.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToListAsync();
    }
}
