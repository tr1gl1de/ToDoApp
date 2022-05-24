namespace ToDoApp.Domain.Exceptions;

public class NoteDoesNotBelongToUserException : BadRequestException
{
    public NoteDoesNotBelongToUserException(Guid userId, Guid noteId)
        : base($"The note with identifier {noteId} does not belong to the user with identifier {userId}")
    {
    }
}