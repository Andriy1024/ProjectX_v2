using ProjectX.Identity.Persistence;

namespace ProjectX.Identity.Application.Handlers;

public sealed class SignUpHandler : ICommandHandler<SignUpCommand, SignUpResult>
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly IdentityDatabase _dbContext;

    public SignUpHandler(
        UserManager<AccountEntity> userManager, 
        IdentityDatabase dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
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

        await _dbContext.SaveChangesAsync();
        
        return new SignUpResult(newUser.Id);
    }
}