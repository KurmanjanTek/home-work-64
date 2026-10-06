using home_work_54_lab_work.Models;
using home_work_54_lab_work.Services;
using Microsoft.AspNetCore.Mvc;

namespace home_work_54_lab_work.Controllers;

public class TestController : Controller
{
    private readonly TaskItemService _taskItemService;


    public TestController(TaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    public async Task<IActionResult> Index(int id)
    {
        TaskItem t = await _taskItemService.GetTaskItem(id);
        if (t != null)
            return Content($"task is " + t.Name);
        return Content("task not found");
    }
    
    
   [ResponseCache( Location = ResponseCacheLocation.Any, Duration =300)]
    public IActionResult Test()
    {
        return View();
    }
}