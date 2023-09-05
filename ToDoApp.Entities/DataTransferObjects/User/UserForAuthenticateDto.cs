using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entities.DataTransferObjects.User;

public class UserForAuthenticateDto
{
    /// <summary>Username for authorization</summary>
    /// <example>John_Doe333</example>
    [Required]
    public string Username { get; set; } = string.Empty;
    
    /// <summary>Password for authorization</summary>
    /// <example>!1Password123</example>
    [Required]
    public string Password { get; set; } = string.Empty;
}