namespace ToDoApp.Entities.DataTransferObjects.Note;

public class NoteForReadDto
{   
    /// <summary>Name of note</summary>
    /// <example>My To-Do list</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Body of note</summary>
    /// <example>Do something</example>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>Date of creation note</summary>
    /// <example>27.05.2022 17:57:02</example>
    public DateTime DateCreation { get; set; }
    
    /// <summary>Date of creation note</summary>
    /// <example>28.05.2022 18:10:35</example>
    public DateTime? DateUpdate { get; set; }
}