namespace ProjectX.Identity.API.Results;

public class AuthResult
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public static AuthResult Success(string token, string refreshToken) => new()
    {
        Token = token,
        RefreshToken = refreshToken
    };
}