namespace ProjectX.Identity.Application.Handlers;

public sealed class SignUpHandler : ICommandHandler<SignUpCommand, SignUpResult>
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly AuthorizationService _jwtService;

    public SignUpHandler(UserManager<AccountEntity> userManager, AuthorizationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ResultOf<SignUpResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
        {
            return ApplicationError.NotFound(message: "Email already in use.");
        }

        var newUser = new AccountEntity()
        {
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = false,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var isCreated = await _userManager.CreateAsync(newUser, request.Password);
        if (isCreated.Succeeded == false)
        {
            var error = string.Join(", ", isCreated.Errors.Select(e => e.Code));

            return ApplicationError.InvalidData(message: error);
        }

        var authResult = await _jwtService.GenerateTokenAsync(newUser);
        if (authResult.IsFailed)
        {
            return authResult.Error!;
        }

        return new SignUpResult(authResult.Data!.Token, authResult.Data!.RefreshToken);
    }
}