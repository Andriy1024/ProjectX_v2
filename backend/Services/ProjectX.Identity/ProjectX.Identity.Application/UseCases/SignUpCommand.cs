namespace ProjectX.Identity.Application;

public sealed class SignUpCommand : ICommand<SignUpResult>, IValidatable
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
    
    public string Password { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.FirstName).NotEmpty();
            command.RuleFor(x => x.LastName).NotEmpty();
            command.RuleFor(x => x.Email).NotEmpty();
            command.RuleFor(x => x.Password).NotEmpty();
        });
    }
}

public record SignUpResult(string Token, string RefreshToken);