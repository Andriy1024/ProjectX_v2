namespace ProjectX.Core.Auth;

public interface ICurrentUser
{
    int IdentityId { get; }
    string IdentityRole { get; }
}