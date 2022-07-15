namespace ProjectX.Authentication;

public class AuthenticationConfig
{
    public string Audience { get; set; }

    public string Issuer { get; set; }

    public string Secret { get; set; }

    public TimeSpan ExpiryTimeFrame { get; set; }
}