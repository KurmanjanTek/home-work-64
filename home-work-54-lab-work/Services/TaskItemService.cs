using home_work_54_lab_work.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace home_work_54_lab_work.Services;

public class TaskItemService
{
    private readonly TaskContext _context;
    private readonly IMemoryCache _cache;

    public TaskItemService(TaskContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasks()
    {
        return await _context.TaskItems.ToListAsync();
    }

    public async Task AddTaskItem(TaskItem item)
    {
        await _context.AddAsync(item);
        int n= await _context.SaveChangesAsync();
        if (n > 0)
        {
            _cache.Set(item.Id, item,
                new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
        }
    }

    public async Task<TaskItem> GetTaskItem(int id)
    {
        TaskItem item = null;
        if (!_cache.TryGetValue(id, out item))
        {
            item = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item != null)
            {
                _cache.Set(item.Id, item,
                    new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }); 
            }
        }

        return item;
    }
}