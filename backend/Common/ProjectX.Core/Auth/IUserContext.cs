namespace ProjectX.Core.Auth;

public interface IUserContext
{
    int Id { get; }

    string Role { get; }
}