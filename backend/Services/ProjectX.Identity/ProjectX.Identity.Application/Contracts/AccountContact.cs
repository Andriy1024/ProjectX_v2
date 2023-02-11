namespace ProjectX.Identity.Application.Contracts;

public record AccountContact
{
    public int Id { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
}