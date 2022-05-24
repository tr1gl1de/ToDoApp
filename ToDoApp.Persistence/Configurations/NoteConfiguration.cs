using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(note => note.Id);

        builder.Property(note => note.Id)
            .ValueGeneratedOnAdd();

        builder.Property(note => note.Name)
            .IsRequired();

        builder.Property(note => note.Description)
            .IsRequired();

        builder.Property(note => note.DateCreation)
            .IsRequired();

        builder.Property(note => note.DateUpdate);
    }
}