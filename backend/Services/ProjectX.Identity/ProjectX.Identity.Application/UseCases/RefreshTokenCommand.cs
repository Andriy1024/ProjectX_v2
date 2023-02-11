namespace ProjectX.Identity.Application;

public sealed class RefreshTokenCommand : ICommand<RefreshTokenResult>, IValidatable
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command => 
        {
            command.RuleFor(x => x.Token).NotEmpty();
            command.RuleFor(x => x.RefreshToken).NotEmpty();
        });
    }
}

public record RefreshTokenResult(string Token, string RefreshToken);