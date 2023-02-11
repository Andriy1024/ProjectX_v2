using Microsoft.EntityFrameworkCore;
using ProjectX.Core.Auth;
using ProjectX.Identity.Application.Contracts;
using ProjectX.Identity.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Identity.Application.Handlers;

public sealed class GetAccountInfoHandler : IQueryHandler<GetAccountInfoQuery, AccountContact>
{
    private readonly IUserContext _user;
    private readonly UserManager<AccountEntity> _userManager;
    private readonly IMapper _mapper;

    public GetAccountInfoHandler(IUserContext user, UserManager<AccountEntity> userManager, IMapper mapper)
    {
        _user = user;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ResultOf<AccountContact>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var accountEntity = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == _user.Id, cancellationToken);

        return accountEntity == null
            ? ApplicationError.NotFound(message: "User not found")
            : _mapper.Map<AccountContact>(accountEntity);
    }
}
