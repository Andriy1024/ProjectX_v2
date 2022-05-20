namespace ProjectX.Core;

public interface IPaginationOptions
{
    int Skip { get; }

    int Take { get; }
}
