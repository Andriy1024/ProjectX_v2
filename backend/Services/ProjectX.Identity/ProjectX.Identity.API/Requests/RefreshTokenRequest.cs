using System.ComponentModel.DataAnnotations;

namespace ProjectX.Identity.API.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
