using ProjectX.Core;

namespace ProjectX.Dashboard.Domain.Entities;

public sealed class BookmarkEntity : Entity<int>
{
    public override int Id { get; protected set; }

    public string Name { get; private set; }

    public string URL { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }


    #pragma warning disable CS8618
    /// <summary>
    /// For EF.
    /// </summary>
    private BookmarkEntity() {}
    #pragma warning restore CS8618

    public static BookmarkEntity Create(string name, string url) 
    {
        var bookmarkEntity = new BookmarkEntity() 
        {
            Name = name,
            URL = url,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now,
        };

        bookmarkEntity.AddDomainEvent(new EntityCreated<BookmarkEntity>(bookmarkEntity));

        return bookmarkEntity;
    }

    public void Update(string name, string url)
    {
        Name = name;
        URL = url;
        UpdatedAt = DateTimeOffset.Now;
        AddDomainEvent(new EntityUpdated<BookmarkEntity>(this));
    }

    public void Remove()
    {
        AddDomainEvent(new EntityDeleted<BookmarkEntity>(this));
    }
}