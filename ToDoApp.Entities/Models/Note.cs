namespace ToDoApp.Entities.Models;

public class Note
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public DateTime? DateUpdate { get; set; }
}