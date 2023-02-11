namespace ProjectX.Identity.Application;

public sealed class SignInCommand : ICommand<SignInResult>, IValidatable
{
    public string Email { get; set; }

    public string Password { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Email).NotEmpty();
            command.RuleFor(x => x.Password).NotEmpty();
        });
    }
}

public record SignInResult(string Token, string RefreshToken);