namespace ToDoApp.Domain.Exceptions;

public class NoteNotFoundException : NotFoundException
{
    public NoteNotFoundException(Guid id)
        : base($"The note with the identifier {id} was not found.")
    {
        
    }
}