namespace ProjectX.Identity.Application.Handlers;

public sealed class SignInHandler : ICommandHandler<SignInCommand, SignInResult>
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly AuthorizationService _jwtService;

    public SignInHandler(UserManager<AccountEntity> userManager, AuthorizationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ResultOf<SignInResult>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist == null)
        {
            return ApplicationError.NotFound(message: "User not found");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExist, command.Password);

        if (isPasswordCorrect == false)
        {
            return ApplicationError.InvalidData(message: "Invalid password");
        }

        var authResult = await _jwtService.GenerateTokenAsync(userExist);

        if (authResult.IsFailed)
        {
            return authResult.Error!;
        }

        return new SignInResult(authResult.Data!.Token, authResult.Data!.RefreshToken);
    }
}