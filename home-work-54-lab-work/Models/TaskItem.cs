using home_work_54_lab_work.Enums;

namespace home_work_54_lab_work.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Priority Prioritety { get; set; }
    public ToDoStatus Status { get; set; }
    public string ExecuterName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
}