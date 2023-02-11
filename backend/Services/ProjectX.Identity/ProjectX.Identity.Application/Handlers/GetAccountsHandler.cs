using Microsoft.EntityFrameworkCore;
using ProjectX.Identity.Application.Contracts;
using ProjectX.Identity.Application.UseCases;

namespace ProjectX.Identity.Application.Handlers;

public sealed class GetAccountsHandler : IQueryHandler<GetAccountsQuery, IEnumerable<AccountContact>>
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly IMapper _mapper;

    public GetAccountsHandler(UserManager<AccountEntity> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ResultOf<IEnumerable<AccountContact>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accountEntities = await _userManager.Users.ToArrayAsync(cancellationToken);

        return _mapper.Map<AccountContact[]>(accountEntities);
    }
}
