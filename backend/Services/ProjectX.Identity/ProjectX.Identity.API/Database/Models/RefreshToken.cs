namespace ProjectX.Identity.API.Database.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public string Token { get; set; }
    
    public string JwtId { get; set; }
    
    public bool IsUsed { get; set; }
    
    public bool IsRevorked { get; set; }
    
    public DateTime AddedDate { get; set; }
    
    public DateTime ExpiryDate { get; set; }

    public UserEntity User { get; set; }
}