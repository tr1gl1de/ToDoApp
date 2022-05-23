namespace ToDoApp.Contracts;

public class NoteForReadDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateUpdate { get; set; }
}