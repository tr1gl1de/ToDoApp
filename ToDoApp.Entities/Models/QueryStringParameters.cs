namespace ToDoApp.Entities.Models;

public abstract class QueryStringParameters
{
    const int maxPageSize = 50;
    
    /// <summary>Number of page</summary>
    /// <example>1</example>
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    /// <summary>Size of page</summary>
    /// <example>3</example>
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
    
    /// <summary>Sort field name</summary>
    /// <example>name</example>
    public string OrderBy { get; set; }
}