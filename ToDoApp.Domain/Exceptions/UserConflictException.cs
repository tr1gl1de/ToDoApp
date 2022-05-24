namespace ToDoApp.Domain.Exceptions;

public class UserConflictException : ConflictException
{
    public UserConflictException(string username)
        : base($"Username \"{username}\" already taken")
    {
    }

    public UserConflictException()
        : base($"Username already taken")
    {
        
    }
}