using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Identity.API.Database.Models;

namespace ProjectX.Identity.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<AccountEntity> _userManager;

    public AccountsController(UserManager<AccountEntity> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAccounts() 
    {
        var users = await _userManager.Users.Select(u => new
            {
                u.Id,
                u.Email,
                u.EmailConfirmed,
                u.FirstName,
                u.LastName
            })
            .ToListAsync();

        return Ok(users);
    }
}