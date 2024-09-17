using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyToDo.Data;
using MyToDo.Models;
using System.Security.Claims;

namespace MyToDo.Controllers
{
    [Authorize]
    public class ToDoTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToDoTasksController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Tasks.
                Where(t => t.UserId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();
            return View(model);
        }

        // GET request
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, StatusCode, DueDate, UserId")] ToDoTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET request
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ToDoTask task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(); // may be change to task does not exist
            }
            if (task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound(); // may be change to access deny
            }
            return View(task);
        }

        // POST request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, StatusCode, DueDate, UserId")] ToDoTask task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Tasks.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (IsTaskExist(id))
                    {
                        throw;
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(task); // doesn't really obvious when something went wrong
        }

        // GET request
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // ToDoTask task = await _context.Tasks.FindAsync(id);
            ToDoTask task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(tk => tk.Id == id); // a little bit strange but will try
            if (task == null)
            {
                return NotFound(); // May be change to the task does not exist
            }
            if (task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound(); // May be change to access denied
            }
            return View(task);
        }

        // POST request
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            ToDoTask? task = await _context.Tasks.FindAsync(id);
            if (task != null) // May be add task is already deleted
            {
                _context.Tasks.Remove(task);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool IsTaskExist(int id)
        {
            return _context.Tasks.Any(task => task.Id == id);
        }
    }
}
