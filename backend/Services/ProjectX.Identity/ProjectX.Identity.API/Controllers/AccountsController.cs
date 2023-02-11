using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Identity.Application.UseCases;
using ProjectX.Identity.Domain;

namespace ProjectX.Identity.API.Controllers;

[Route("api/accounts")]
public class AccountsController : ProjectXController
{
    [HttpGet("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> GetAccountInfo(CancellationToken cancellation)
    {
        return Send(new GetAccountInfoQuery(), cancellation);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> GetAccounts(CancellationToken cancellation) 
    {
        return Send(new GetAccountsQuery(), cancellation);
    }
}