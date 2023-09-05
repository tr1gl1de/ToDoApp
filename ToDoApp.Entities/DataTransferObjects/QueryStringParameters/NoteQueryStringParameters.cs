namespace ToDoApp.Entities.DataTransferObjects.QueryStringParameters;

public class NoteQueryStringParameters : Models.QueryStringParameters
{
    public NoteQueryStringParameters()
    {
        OrderBy = "name";
    }
}