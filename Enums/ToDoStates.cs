using System.ComponentModel.DataAnnotations;

namespace MyToDo.Enums
{
    public enum ToDoStates
    {
        [Display(Name = "To do")]
        ToDo = 0,
        [Display(Name = "In progress")]
        InProgress = 1,
        [Display(Name = "Done")]
        Done = 2
    };
}
