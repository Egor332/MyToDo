using Microsoft.AspNetCore.Identity;
using MyToDo.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyToDo.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Status")]
        public byte? StatusCode { get; set; }
        [Display(Name="Due date")]
        public DateOnly? DueDate { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

    }
}
