using ProjectX.Identity.Application.Contracts;

namespace ProjectX.Identity.Application.Mapper;

public class IdentityProfile : Profile
{
	public IdentityProfile()
	{
		CreateMap<AccountEntity, AccountContact>();
        CreateMap<TokenResult, TokenContact>();
    }
}