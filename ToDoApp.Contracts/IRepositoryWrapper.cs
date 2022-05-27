namespace ToDoApp.Contracts;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }
    INoteRepository Note { get; }
    Task SaveAsync();
}