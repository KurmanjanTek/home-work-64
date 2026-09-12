using Microsoft.EntityFrameworkCore;

namespace home_work_54_lab_work.Models;

public class TaskContext :DbContext
{
    public DbSet<TaskItem> TaskItems { get; set; }
    
    
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) {}
}