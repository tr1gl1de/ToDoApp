using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Entities.Models;

namespace ToDo.Persistence.Configurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        // for real db provider
        // entity.ToTable("users");

        entity
            .HasKey(user => user.Id);
        
        entity
            .HasIndex(user => user.Username)
            .IsUnique();
        
        entity
            .Property(user => user.Id)
            .IsRequired();

        entity
            .Property(note => note.Id)
            .ValueGeneratedOnAdd();

        entity
            .Property(user => user.Username)
            .IsRequired();

        entity
            .Property(user => user.DisplayName)
            .IsRequired();

        entity
            .Property(user => user.Password)
            .IsRequired();

        entity
            .HasMany(user => user.Notes)
            .WithOne()
            .HasForeignKey(note => note.UserId);
    }
}