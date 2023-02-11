namespace ProjectX.Identity.Application.Contracts;

public record TokenContact
{
    public string Token { get; init; }

    public string RefreshToken { get; init; }
}