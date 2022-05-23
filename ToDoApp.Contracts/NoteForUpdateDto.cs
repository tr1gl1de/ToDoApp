namespace ToDoApp.Contracts;

public class NoteForUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateUpdate { get; set; }
}