using System.ComponentModel.DataAnnotations;

namespace ProjectX.Authentication;

public class AuthenticationConfig
{
    [Required]
    public string Audience { get; set; }

    [Required]
    public string Issuer { get; set; }

    [Required]
    public string Secret { get; set; }

    [Required]
    public TimeSpan ExpiryTimeFrame { get; set; }
}