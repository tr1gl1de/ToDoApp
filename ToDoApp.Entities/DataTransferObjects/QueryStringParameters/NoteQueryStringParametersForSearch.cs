namespace ToDoApp.Entities.DataTransferObjects.QueryStringParameters;

public class NoteQueryStringParametersForSearch : Models.QueryStringParameters
{
    public NoteQueryStringParametersForSearch()
    {
        OrderBy = "name";
    }
    
    /// <summary>Name for search</summary>
    /// <example>my note</example>
    public string Name { get; set; } = string.Empty;
}