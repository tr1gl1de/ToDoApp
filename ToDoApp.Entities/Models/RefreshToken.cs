namespace ToDoApp.Entities.Models;

public class RefreshToken
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public DateTime ExpirationTime { get; set; }
}