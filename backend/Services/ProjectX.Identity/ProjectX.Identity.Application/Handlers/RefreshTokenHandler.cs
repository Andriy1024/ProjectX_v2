namespace ProjectX.Identity.Application.Handlers;

public sealed class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly AuthorizationService _jwtService;

    public RefreshTokenHandler(AuthorizationService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<ResultOf<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _jwtService.RefreshTokenAsync(request);

        if (authResult.IsFailed)
        {
            return authResult.Error!;
        }

        var jwtToken = authResult.Data!;

        return new RefreshTokenResult(jwtToken.Token, jwtToken.RefreshToken.Token);
    }
}