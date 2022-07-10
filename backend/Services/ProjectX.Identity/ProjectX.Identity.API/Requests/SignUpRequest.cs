using System.ComponentModel.DataAnnotations;

namespace ProjectX.Identity.API.Requests;

public class SignUpRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}