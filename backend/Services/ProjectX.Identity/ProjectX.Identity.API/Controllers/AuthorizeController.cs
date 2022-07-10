using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Core;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;

namespace ProjectX.Identity.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly AuthorizationService _jwtService;

    public AuthorizeController(UserManager<AccountEntity> userManager, AuthorizationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpPost("sign-in")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);

        if (userExist == null)
        {
            return NotFound(Error.NotFound(message: "User not found"));
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExist, request.Password);

        if (isPasswordCorrect == false)
        {
            return BadRequest(Error.InvalidData(message: "Invalid password"));
        }

        var authResult = await _jwtService.GenerateTokenAsync(userExist);

        return authResult.IsFailed
            ? BadRequest(authResult)
            : Ok(authResult);
    }

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Error.InvalidData(message: "Invalid payload"));
        }

        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
        {
            return BadRequest(Error.NotFound(message: "Email already in use."));
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
            return BadRequest(new
            {
                Errors = isCreated.Errors.Select(e => e.Code).ToArray()
            });
        }

        var authResult = await _jwtService.GenerateTokenAsync(newUser);

        return authResult.IsFailed
            ? BadRequest(authResult)
            : Ok(authResult);
    }

    [HttpPost("refresh-token")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Error.InvalidData(message: "Invalid payload"));
        }

        var authResult = await _jwtService.RefreshTokenAsync(request);

        return authResult.IsFailed
           ? BadRequest(authResult)
           : Ok(authResult);
    }
}