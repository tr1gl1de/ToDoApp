namespace ToDoApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public DateTime DateCreation { get; set; }
    public ICollection<Note> Notes { get; set; }
}