using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entities.DataTransferObjects.Note;

public class NoteForUpdateDto
{   
    /// <summary>Name of note</summary>
    /// <example>My To-Do list</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Body of note</summary>
    /// <example>Do something</example>
    [Required]
    public string Description { get; set; } = string.Empty;
}