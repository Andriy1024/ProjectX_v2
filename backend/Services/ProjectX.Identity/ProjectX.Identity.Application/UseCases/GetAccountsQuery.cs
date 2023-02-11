using ProjectX.Identity.Application.Contracts;

namespace ProjectX.Identity.Application.UseCases;

public sealed class GetAccountsQuery : IQuery<IEnumerable<AccountContact>>
{
}