namespace ProjectX.Identity.Domain;

public class RefreshToken
{
    public int Id { get; private set; }

    public required string Token { get; init; }

    public required string JwtId { get; init; }

    public required bool IsUsed { get; set; }
    
    public required bool IsRevorked { get; set; }
    
    public required DateTime AddedDate { get; init; }
    
    public required DateTime ExpiryDate { get; init; }

    public required int UserId { get; init; }

    public required AccountEntity User { get; init; }

    public static RefreshToken Create(AccountEntity account, string jwtId, DateTime issuedAt)
    {
        return new RefreshToken()
        {
            JwtId = jwtId,
            IsUsed = false,
            IsRevorked = false,
            UserId = account.Id,
            User = account,
            AddedDate = issuedAt,
            ExpiryDate = issuedAt.AddMonths(6),
            Token = RandomString(35)
        };
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
}