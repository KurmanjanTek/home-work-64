using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace home_work_54_lab_work.Models;

public class TaskContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User>Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Comment> Comments {get; set;}
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) {}
}