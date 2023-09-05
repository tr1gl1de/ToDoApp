using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entities.DataTransferObjects.User;

public class UserForRegisterDto
{
    /// <summary>Username for authorization</summary>
    /// <example>John_Doe333</example>
    [Required]
    public string Username { get; set; } = string.Empty;
    
    /// <summary>User display name</summary>
    /// <example>John Doe</example>
    [Required]
    public string DisplayName { get; set; } = string.Empty;
    
    /// <summary>Password for authorization</summary>
    /// <example>!1Password123</example>
    [Required]
    public string Password { get; set; } = string.Empty;
}