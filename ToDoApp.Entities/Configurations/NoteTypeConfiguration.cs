using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Entities.Models;

namespace ToDoApp.Entities.Configurations;

public class NoteTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> entity)
    {
        entity
            .HasKey(note => note.Id);

        entity
            .Property(note => note.Id)
            .ValueGeneratedOnAdd();

        entity
            .Property(note => note.Name)
            .IsRequired();

        entity
            .Property(note => note.Description);

        entity
            .Property(note => note.DateCreation)
            .IsRequired();

        entity
            .Property(note => note.DateUpdate);
    }
}