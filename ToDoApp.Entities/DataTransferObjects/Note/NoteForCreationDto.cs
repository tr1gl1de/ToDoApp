using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entities.DataTransferObjects.Note;

public class NoteForCreationDto
{
    /// <summary>User identifier</summary>
    /// <example>3B0C6B8E-271A-4CEE-86E8-B0C4B2747316</example>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>Name of note</summary>
    /// <example>My To-Do list</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Body of note</summary>
    /// <example>Do something</example>
    [Required]
    public string Description { get; set; } = string.Empty;
}