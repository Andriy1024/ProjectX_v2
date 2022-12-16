using ProjectX.Core;

namespace ProjectX.Dashboard.Domain.Entities;

public sealed class NoteEntity : Entity<int>
{
    public override int Id { get; protected set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    #pragma warning disable CS8618
    /// <summary>
    /// For EF.
    /// </summary>
    private NoteEntity() { }
    #pragma warning restore CS8618

    public static NoteEntity Create(string title, string content) 
    {
        var note = new NoteEntity()
        {
            Title = title,
            Content = content,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        note.AddDomainEvent(new EntityCreated<NoteEntity>(note));

        return note;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedAt= DateTimeOffset.Now;
        AddDomainEvent(new EntityUpdated<NoteEntity>(this));
    }

    public void Remove()
    {
        AddDomainEvent(new EntityDeleted<NoteEntity>(this));
    }
}