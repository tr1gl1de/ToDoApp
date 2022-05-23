namespace ToDoApp.Contracts;

public class UserForReadDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public DateTime DateCreation { get; set; }
    public ICollection<NoteForReadDto> Notes { get; set; }
}