namespace ToDoApp.Entities.DataTransferObjects.User;

public class UserForReadDto
{
    /// <summary>User identifier</summary>
    /// <example>3B0C6B8E-271A-4CEE-86E8-B0C4B2747316</example>
    public Guid Id { get; set; }
    
    /// <summary>Username for authorization</summary>
    /// <example>John_Doe333</example>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>User display name</summary>
    /// <example>John Doe</example>
    public string DisplayName { get; set; } = string.Empty;
}