using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using home_work_54_lab_work.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using home_work_54_lab_work.Models;
using home_work_54_lab_work.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace home_work_54_lab_work.Controllers
{
    [Authorize]
    public class TaskItemController : Controller
    {
        private readonly TaskContext _context;
        private readonly UserManager<User> _userManager;

        public TaskItemController(TaskContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskItem
        public async Task<IActionResult> Index(ToDoTaskSortState sortOrder, string name, string description, DateTime? dateFrom, DateTime? dateTo, Priority? priority, ToDoStatus? status, int page=1)
        {
            ViewBag.NameSort=sortOrder==ToDoTaskSortState.NameAsc ? ToDoTaskSortState.NameDesc : ToDoTaskSortState.NameAsc;
            ViewBag.PrioritySort=sortOrder==ToDoTaskSortState.PriorityAsc ? ToDoTaskSortState.PriorityDesc : ToDoTaskSortState.PriorityAsc;
            ViewBag.StatusSort=sortOrder==ToDoTaskSortState.StatusAsc ? ToDoTaskSortState.StatusDesc : ToDoTaskSortState.StatusAsc;
            ViewBag.CreatedDateSort=sortOrder==ToDoTaskSortState.CreatedDateAsc ? ToDoTaskSortState.CreatedDateDesc : ToDoTaskSortState.CreatedDateAsc;
            var tasks=_context.TaskItems.ToList();
            
            if (!string.IsNullOrEmpty(name))
            {
                tasks = tasks.Where(t => t.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                tasks = tasks.Where(t => t.Description.ToLower().Contains(description.ToLower())).ToList();
            }

            if (dateFrom.HasValue)
            {
                tasks = tasks.Where(t => t.CreatedDate >= dateFrom.Value).ToList();
            }
            
            if (dateTo.HasValue)
            {
                tasks = tasks.Where(t => t.CreatedDate <= dateTo.Value).ToList();
            }
            
            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status.Value).ToList();
            }
            
            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Prioritety == priority.Value).ToList();
            }

            switch (sortOrder)
            {
                case ToDoTaskSortState.NameAsc:
                    tasks=tasks.OrderBy(t => t.Name).ToList();
                    break;
                case ToDoTaskSortState.NameDesc:
                    tasks=tasks.OrderByDescending(t => t.Name).ToList();
                    break;
                case ToDoTaskSortState.PriorityAsc:
                    tasks=tasks.OrderBy(t => t.Prioritety).ToList();
                    break;
                case ToDoTaskSortState.PriorityDesc:
                    tasks=tasks.OrderByDescending(t => t.Prioritety).ToList();
                    break;
                case ToDoTaskSortState.StatusAsc:
                    tasks=tasks.OrderBy(t => t.Status).ToList();
                    break;
                case ToDoTaskSortState.StatusDesc:
                    tasks=tasks.OrderByDescending(t => t.Status).ToList();
                    break;
                case ToDoTaskSortState.CreatedDateAsc:
                    tasks=tasks.OrderBy(t => t.CreatedDate).ToList();
                    break;
                case ToDoTaskSortState.CreatedDateDesc:
                    tasks=tasks.OrderByDescending(t => t.CreatedDate).ToList();
                    break;
            }
            int pageSize = 10;
            var count=tasks.Count();
            var items=tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pvm = new PageViewModel(count, page, pageSize);
            TaskIndexViewModel tivm = new TaskIndexViewModel()
            {
                TaskItems = items,
                PageViewModel = pvm
            };
            return View(tivm);
        }

        // GET: TaskItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItem/Create
        public IActionResult Create()
        {
            var model=new TaskItemViewModel();
            return View(model);
        }

        // POST: TaskItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId =int.Parse(_userManager.GetUserId(User)) ;
                var task = new TaskItem
                {
                    Name = model.Name,
                    Description = model.Description,
                    Prioritety = model.Prioritety,
                    Status=ToDoStatus.New,
                    CreatorId =userId,
                    CreatedDate=DateTime.UtcNow,
                    ClosedDate= null
                };
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Open(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Status = ToDoStatus.InProgress;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TakeTask(int id)
        {
            var taskItem=await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
                return NotFound();
            
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            
            taskItem.CreatorId=userId;
            taskItem.ExucutorId = userId;
            User.IsInRole("admin");
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Close(int id)
        {
            var taskItem= await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            if (User.IsInRole("admin"))
            {
                taskItem.ClosedDate=DateTime.UtcNow;
                taskItem.Status= ToDoStatus.Done; 
                await _context.SaveChangesAsync();
            }
            

            return RedirectToAction(nameof(Index));
        }

        // GET: TaskItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            if (taskItem.CreatorId == userId || User.IsInRole("admin"))
            {
                return View(taskItem);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: TaskItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Prioritety,Status,CreatedDate,ClosedDate")] TaskItem task)
        {
            var taskItem=await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            var userId =int.Parse(_userManager.GetUserId(User)) ;

            if (taskItem.CreatorId == userId)
            {
                taskItem.Name = task.Name;
                taskItem.Description = task.Description;
                taskItem.Prioritety = task.Prioritety;

                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TaskItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            var userId =int.Parse(_userManager.GetUserId(User)) ;
             if (taskItem.CreatorId == userId || User.IsInRole("admin"))
             {
                 return View(taskItem);
             }

             return RedirectToAction(nameof(Index));
        }

        // POST: TaskItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                NotFound();
            }
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            if (taskItem.CreatorId == userId || User.IsInRole("admin"))
            {
               _context.TaskItems.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeStatus(int id, ToDoStatus status)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                NotFound();
            }
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            if (taskItem.CreatorId == userId || User.IsInRole("admin"))
            {
                taskItem.Status = status;
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyTasks()
        {
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            var tasks=await _context.TaskItems.Where(t=>t.ExucutorId==userId).ToListAsync();
            return View(tasks);
        }
        
        public async Task<IActionResult> FreeTasks()
        {
            var tasks=await _context.TaskItems.Where(t=>t.ExucutorId==null).ToListAsync();
            return View(tasks);
        }
        
        public async Task<IActionResult> ByCreatorTasks()
        {
            var userId =int.Parse(_userManager.GetUserId(User)) ;
            var tasks=await _context.TaskItems.Where(t=>t.CreatorId==userId).ToListAsync();
            return View(tasks);
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
