using Microsoft.EntityFrameworkCore;
using ToDoApp.Entities.Models;

namespace ToDo.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(
                new User
                {
                    Id = Guid.Parse("E6072F67-015B-4A80-B7E8-F89E16BACAB3"),
                    Username = "John_Doe333",
                    DisplayName = "John_doe222",
                    Password = BCrypt.Net.BCrypt.HashPassword("!1Password123")
                }
            );

        modelBuilder.Entity<Note>()
            .HasData(new Note
                {
                    Id = Guid.Parse("4A094225-7082-4BC9-9F56-28B771C0AFBD"),
                    Name = "Test note n1",
                    Description = "My first test note for us",
                    DateCreation = DateTime.UtcNow,
                    UserId = Guid.Parse("E6072F67-015B-4A80-B7E8-F89E16BACAB3")
                },
                new Note
                {
                    Id = Guid.Parse("A5E4C3E5-3C26-4DE2-BF4C-89B75B6A6F75"),
                    Name = "Test note n2",
                    Description = "Some text and so many symboooooooooooooooooooooooooooooooools",
                    DateCreation = DateTime.Parse("2021-08-18T07:22:16.0000000Z"),
                    DateUpdate = DateTime.UtcNow,
                    UserId = Guid.Parse("E6072F67-015B-4A80-B7E8-F89E16BACAB3")
                },
                new Note
                {
                    Id = Guid.Parse("2EFB17C8-87CC-499F-8921-3A9BD21D6153"),
                    Name = "My first note",
                    Description = "My new first note in app",
                    DateCreation = DateTime.Parse("2022-05-18T17:36:55.0000000Z"),
                    UserId = Guid.Parse("E6072F67-015B-4A80-B7E8-F89E16BACAB3")
                });
    }
}