using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Username)
            .IsRequired();

        builder.Property(user => user.Password)
            .IsRequired();

        builder.Property(user => user.DisplayName)
            .IsRequired();

        builder.Property(user => user.DateCreation)
            .IsRequired();

        builder.HasMany(user => user.Notes)
            .WithOne()
            .HasForeignKey(note => note.UserId);
    }
}