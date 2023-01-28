namespace ProjectX.FileStorage.Persistence.Database.Abstractions;

public interface IDocumentEntry<TKey>
{
    public abstract static string Collection { get; }

    public TKey Id { get; }
}