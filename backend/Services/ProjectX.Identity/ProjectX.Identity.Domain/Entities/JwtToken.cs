namespace ProjectX.Identity.Domain;

public partial class JwtToken
{
    public string Id { get; }

    public string Token { get; }

    public DateTime IssuedAt { get; }

    public RefreshToken RefreshToken { get; }

    protected JwtToken(string id, string token, DateTime issuedAt, RefreshToken refreshToken)
    {
        Id = id;
        Token = token;
        IssuedAt = issuedAt;
        RefreshToken = refreshToken;
    }

    public static JwtBuilder Builder => new();
}
