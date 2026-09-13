using home_work_54_lab_work.Models;

namespace home_work_54_lab_work.ViewModels;

public class TaskIndexViewModel
{
    public List<TaskItem> TaskItems {get; set;}
    public PageViewModel PageViewModel { get; set; }
}