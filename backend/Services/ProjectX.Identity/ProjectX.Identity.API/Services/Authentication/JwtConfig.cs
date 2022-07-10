namespace ProjectX.Identity.API.Authentication;

public class JwtConfig
{
    public string Secret { get; set; }

    public TimeSpan ExpiryTimeFrame { get; set; }
}
