using home_work_54_lab_work.Enums;

namespace home_work_54_lab_work.ViewModels;

public class TaskItemViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Priority Prioritety { get; set; }
    public string ExecuterName { get; set; }
}