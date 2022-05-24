namespace ToDoApp.Services.Abstraction;

public interface IServiceManager
{
    IUserService UserService { get; }
    
    INoteService NoteService { get; }
}