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
            var model = await _context.Tasks.Where(t => t.UserId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();
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
        public async Task<IActionResult> Create([Bind("Id, Name, Description, StatusCode, UserId")] ToDoTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(task);
        }
    }
}
