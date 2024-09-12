using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyToDo.Models;

namespace MyToDo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ToDoTask> Tasks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
