namespace ProjectX.Core;

public interface IOrderingOptions
{
    string OrderBy { get; }

    bool Descending { get; }
}